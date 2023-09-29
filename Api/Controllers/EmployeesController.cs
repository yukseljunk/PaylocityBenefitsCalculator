using Api.Dtos.Employee;
using Api.Mappers;
using Api.Models;
using Api.Services;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ApiControllerWithProblemClassified
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
        //return Ok(ModelToDtoMapper.MapEmployeeResponse(getEmployeeResult.Value));


        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = ModelToDtoMapper.MapEmployeeResponse(getEmployeeResult.Value),
            Success = true
        };
        return result;
    }


    [SwaggerOperation(Summary = "Delete employee by id")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        ErrorOr<Deleted> deleteEmployeeResult = await _employeeService.DeleteEmployee(id);

        if (deleteEmployeeResult.IsError)
        {
            return ClassifiedProblem(deleteEmployeeResult.Errors);
        }
        return NoContent();
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

        return CreatedAtAction(nameof(Get), new { id = employee.Id }, ModelToDtoMapper.MapEmployeeResponse(employee));

    }

    [SwaggerOperation(Summary = "Update an employee")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id, GetEmployeeDto request)
    {
        var dependents = new List<ErrorOr<Dependent>>();
        request.Dependents.ToList().ForEach(dependent =>
                dependents.Add(
                    Dependent.Create(
                        dependent.FirstName,
                        dependent.LastName,
                        dependent.DateOfBirth,
                        dependent.Relationship,
                        dependent.Id
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
           dependents.Select(d => d.Value).ToList(), 
           id);

        if (requestToCreateEmployeeResult.IsError) return ClassifiedProblem(requestToCreateEmployeeResult.Errors);
        var employee = requestToCreateEmployeeResult.Value;
        employee.Id = id;

        var updateEmployeeResult = await _employeeService.Update(employee);

        if (updateEmployeeResult.IsError)
        {
            return ClassifiedProblem(updateEmployeeResult.Errors);
        }
        return NoContent();
    }


    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        //task: use a more realistic production approach
        //solution: using service interface that can be easily changed later
        ErrorOr<List<Employee>> getEmployeesResult = await _employeeService.GetEmployees();
        var employees = new List<GetEmployeeDto>();
        getEmployeesResult.Value.ForEach(e => employees.Add(ModelToDtoMapper.MapEmployeeResponse(e)));
               
        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };

        return result;
    }

}
