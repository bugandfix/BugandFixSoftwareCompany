﻿using System.Net;

namespace BugandFixSoftwareCompany.Extensions;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Check if the response has already started
        if (context.Response.HasStarted)
        {
            return Task.CompletedTask; // Return a completed Task
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var response = new
        {
            message = "An unexpected error occurred. Please try again later.",
            details = exception.Message
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
