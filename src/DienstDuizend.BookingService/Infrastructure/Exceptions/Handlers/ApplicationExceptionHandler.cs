using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ApplicationException = DienstDuizend.BookingService.Infrastructure.Exceptions.ApplicationException;

namespace DienstDuizend.BookingService.Infrastructure.Exceptions.Handlers;

public class ApplicationExceptionHandler(ILogger<ApplicationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ApplicationException customException)
        {
            return false;
        }

        logger.LogError(
            customException,
            "Exception occurred: {Message}",
            customException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = (int)customException.StatusCode,
            Title = customException.ErrorCode,
            Detail = customException.ErrorMessage,
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}