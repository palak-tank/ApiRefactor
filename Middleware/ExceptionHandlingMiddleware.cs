using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ApiRefactor.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            ArgumentNullException or ArgumentException => (HttpStatusCode.BadRequest, "Invalid request"),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found"),
            UnauthorizedAccessException => (HttpStatusCode.Forbidden, "Access denied"),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        _logger.LogError(
            exception,
            "Unhandled exception {ExceptionType} on {Method} {Path}. TraceId: {TraceId}",
            exception.GetType().Name,
            context.Request.Method,
            context.Request.Path,
            context.TraceIdentifier);

        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7807",
            Title = title,
            Status = (int)statusCode,
            Detail = _env.IsDevelopment() ? exception.Message : title,
            Extensions = { ["traceId"] = context.TraceIdentifier }
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
    }
}
