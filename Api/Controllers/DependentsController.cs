using Api.Dtos.Dependent;
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
       
        return new ApiResponse<GetDependentDto>
        {
            Data = ModelToDtoMapper.MapDependentResponse(getDependentResult.Value),
            Success = true
        };

    }

    [SwaggerOperation(Summary = "Delete dependent by id")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDependent(int id)
    {
        ErrorOr<Deleted> deleteDependentResult = await _dependentService.DeleteDependent(id);
        if (deleteDependentResult.IsError)
        {
            return ClassifiedProblem(deleteDependentResult.Errors);
        }
        return NoContent();
    }


    [SwaggerOperation(Summary = "Update a dependent")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDependent(int id, GetDependentDto request)
    {

        ErrorOr<Dependent> requestToCreateDependentResult = Dependent.Create(
                        request.FirstName,
                        request.LastName,
                        request.DateOfBirth,
                        request.Relationship,
                        request.Id
                    );

        if (requestToCreateDependentResult.IsError)
        {
            return ClassifiedProblem(requestToCreateDependentResult.Errors);
        }

        var dependent = requestToCreateDependentResult.Value;

        var updateDependentResult = await _dependentService.Update(dependent);
        if (updateDependentResult.IsError)
        {
            return ClassifiedProblem(updateDependentResult.Errors);
        }
        return NoContent();
    }



    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        ErrorOr<List<Dependent>> getDependentResult = await _dependentService.GetDependents();
        var dependents = new List<GetDependentDto>();
        getDependentResult.Value.ForEach(e => dependents.Add(ModelToDtoMapper.MapDependentResponse(e)));


        //task: use a more realistic production approach
        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return result;
    }

}
