using Api.Dtos.Employee;
using Api.Models;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Api.Controllers;

public class ApiControllerWithProblemClassified<T> : ControllerBase
{
    protected ActionResult<ApiResponse<T>> ClassifiedProblem(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelStateDictionary);
        }

        if (errors.Any(e => e.Type == ErrorType.Unexpected))
        {
            return Problem();
        }

        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };

        return Problem(statusCode: (int)statusCode, title: firstError.Description);
    }
}
