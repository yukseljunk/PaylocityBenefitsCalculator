using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Api.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ApiControllerWithProblemClassified<GetEmployeeDto>
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        ErrorOr<Employee> getEmployeeResult = await _employeeService.GetEmployee(id);

        if (getEmployeeResult.IsError) return ClassifiedProblem(getEmployeeResult.Errors);
        return Ok(MapEmployeeResponse(getEmployeeResult.Value));
    }

    [SwaggerOperation(Summary = "Create new employee")]
    [HttpPost()]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Create(GetEmployeeDto request)
    {
        var dependents = new List<ErrorOr<Dependent>>();
        request.Dependents.ToList().ForEach(dependent =>
                dependents.Add(
                    Dependent.Create(
                        dependent.FirstName,
                        dependent.LastName,
                        dependent.DateOfBirth,
                        dependent.Relationship
                    )
                )
        );

        var dependentError = dependents.Any(d => d.IsError);

        if (dependentError) return ClassifiedProblem(dependents.SelectMany(d => d.Errors).ToList());

        ErrorOr<Employee> requestToCreateEmployeeResult = Employee.Create(
           request.FirstName,
           request.LastName,
           request.Salary,
           request.DateOfBirth,
           dependents.Select(d => d.Value).ToList());

        if (requestToCreateEmployeeResult.IsError) return ClassifiedProblem(requestToCreateEmployeeResult.Errors);
        var employee = requestToCreateEmployeeResult.Value;
        
        ErrorOr<Created> createBreakFastResult = await _employeeService.CreateEmployee(employee);
        if (createBreakFastResult.IsError) return ClassifiedProblem(createBreakFastResult.Errors);

        return CreatedAtAction(nameof(Get), new { id = employee.Id }, MapEmployeeResponse(employee));

    }


    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        //task: use a more realistic production approach
        var employees = new List<GetEmployeeDto>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };

        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };

        return result;
    }

    // TODO : move this method to some other class for SRP
    private static GetEmployeeDto MapEmployeeResponse(Employee employee)
    {
        var dependents = new List<GetDependentDto>();
        employee.Dependents.ToList().ForEach(dependent => dependents.Add(
                new GetDependentDto()
                {
                    Id = dependent.Id,
                    FirstName = dependent.FirstName,
                    LastName = dependent.LastName,
                    DateOfBirth = dependent.DateOfBirth,
                    Relationship = dependent.Relationship
                }
            ));
        return new GetEmployeeDto()
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth = employee.DateOfBirth,
            Dependents = dependents,
            Salary = employee.Salary
        };
    }

}
