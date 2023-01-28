using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using MyBlog.Service.Exception;

namespace MyBlog.Middlewares.ExceptionHandling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private const string InternalServerErrorMessage = "Internal Server Error";

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                ex, httpContext);
        }
    }

    private async Task HandleExceptionAsync(
        Exception ex, HttpContext context)
    {
        _logger.LogError(ex.Message);

        var response = context.Response;
        
        response.ContentType = MediaTypeNames.Application.Json;

        ErrorDto errorDto = new();
        
        if (ex is CustomException customException)
        {
            response.StatusCode = (int)customException.ResponseStatusCode;
            errorDto.ErrorMessage = customException.Message;
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            errorDto.ErrorMessage = InternalServerErrorMessage;
        }


        await response.WriteAsJsonAsync(errorDto, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }
}