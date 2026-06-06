using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.Exceptions;

namespace Phymnary.SugarPot.AspNetCore.Api.ExceptionHandler;

public class AspExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        HttpStatusCode status;
        string? errorCode = null;
        var message = exception.Message;
        IEnumerable<EntityValidationFailureDetail>? failureDetails = null;

        switch (exception)
        {
            case IBusinessException businessException:
                status = businessException.StatusCode;
                errorCode = businessException.ErrorCode;
                if (businessException is EntityValidationException validationException)
                    failureDetails = validationException.Failures;
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                break;
        }

        if (
            errorCode is not null
            && httpContext.RequestServices.GetService<IAspErrorMessageProvider>()
                is { } messageProvider
        )
        {
            message = await messageProvider.GetAsync(errorCode, cancellationToken);
        }

        httpContext.Response.StatusCode = (int)status;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(
                new AspErrorDto
                {
                    Error = new AspErrorMessage
                    {
                        Code = errorCode,
                        Message = message,
                        Detail = exception.ToString(),
                        InvalidParameters = failureDetails,
                    },
                },
                ApiSerializerContext.Default.AspErrorDto
            ),
            cancellationToken
        );

        return true;
    }
}
