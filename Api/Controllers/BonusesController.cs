using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BonusesController : ApiControllerWithProblemClassified
{
    private readonly IEmployeeService _employeeService;
    public BonusesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [SwaggerOperation(Summary = "Get employee bonus by week number")]
    [HttpGet("{employeeId:int}/week/{weekNo:int}")]
    public async Task<ActionResult<ApiResponse<GetBonusDto>>> Get(int employeeId, int weekNo)
    {
        ErrorOr<Employee> getEmployeeResult = await _employeeService.GetEmployee(employeeId);

        if (getEmployeeResult.IsError) return ClassifiedProblem(getEmployeeResult.Errors);

        throw new Exception("Need to figure out the rest...");
        //return Ok(ModelToDtoMapper.MapEmployeeResponse(getEmployeeResult.Value));
    }


}
