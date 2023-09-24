using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ApiControllerWithProblemClassified
{
    private readonly IDependentService _dependentService;

    public DependentsController(IDependentService dependentService)
    {
        _dependentService = dependentService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        ErrorOr<Dependent> getDependentResult = await _dependentService.GetDependent(id);

        if (getDependentResult.IsError) return ClassifiedProblem(getDependentResult.Errors);
        return Ok(MapDependentResponse(getDependentResult.Value));
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        ErrorOr<List<Dependent>> getDependentResult = await _dependentService.GetDependents();
        var dependents = new List<GetDependentDto>();
        getDependentResult.Value.ForEach(e => dependents.Add(MapDependentResponse(e)));


        //task: use a more realistic production approach
        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return result;
    }


    // TODO : move this method to some other class for SRP, call from other map function
    private static GetDependentDto MapDependentResponse(Dependent dependent)
    {
        return new GetDependentDto()
        {
            Id = dependent.Id,
            FirstName = dependent.FirstName,
            LastName = dependent.LastName,
            DateOfBirth = dependent.DateOfBirth,
            Relationship = dependent.Relationship
        };


    }

}
