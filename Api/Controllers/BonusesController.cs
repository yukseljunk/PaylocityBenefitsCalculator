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
public class BonusesController : ApiControllerWithProblemClassified
{
    private readonly IEmployeeService _employeeService;
    private readonly IBonusService _bonusService;
    public BonusesController(IEmployeeService employeeService, IBonusService bonusService)
    {
        _employeeService = employeeService;
        _bonusService = bonusService;
    }

    [SwaggerOperation(Summary = "Get employee bonus by week number")]
    [HttpGet("{employeeId:int}/week/{weekNo:int}")]
    public async Task<ActionResult<ApiResponse<GetBonusDto>>> Get(int employeeId, int weekNo)
    {
        ErrorOr<Employee> getEmployeeResult = await _employeeService.GetEmployee(employeeId);

        if (getEmployeeResult.IsError) return ClassifiedProblem(getEmployeeResult.Errors);

        var employee = getEmployeeResult.Value;
        var bonusResult = await _bonusService.CalculateBonus(employee, weekNo);
        if (bonusResult.IsError) return ClassifiedProblem(bonusResult.Errors);

        return new ApiResponse<GetBonusDto>
        {
            Data = ModelToDtoMapper.MapBonusResponse(bonusResult.Value),
            Success = true
        };

    }


}
