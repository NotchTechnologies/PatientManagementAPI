using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PatientManagementAPI.Exceptions;

namespace PatientManagementAPI.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
         private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "An unexpected error occurred. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        var (statusCode, title) = exception switch
        {
            DuplicateEmailException =>
                (StatusCodes.Status409Conflict, "Duplicate email"),

            _ =>
                (StatusCodes.Status500InternalServerError,
                 "Internal server error")
        };

        httpContext.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == 500
                ? "An unexpected error occurred."
                : exception.Message,
            Instance = httpContext.Request.Path
        };

        problem.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        await httpContext.Response.WriteAsJsonAsync(
            problem,
            cancellationToken);

        return true;
    }
    }
}