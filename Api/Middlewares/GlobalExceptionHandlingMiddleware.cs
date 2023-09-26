using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;

    }


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, exc.Message);
            context.Response.StatusCode = context.Response.StatusCode;
            context.Response.Headers.Add("GlobalExceptionHandlingMiddleware", exc.Message);

            ProblemDetails problem = new()
            {
                Status = context.Response.StatusCode,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server error occured"
            };
            string json = JsonSerializer.Serialize(problem);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);




        }
    }
}