using Checkin.Common.Api.Middlewares;
using Checkin.Common.Api.Models;
using Microsoft.AspNetCore.Builder;

namespace Checkin.Common.Api.Extensions;

public static class ExceptionsHandlerExtensions
{
    public static IApplicationBuilder AddGlobalExceptionHandling(this WebApplication app,
        Action<ExceptionHandlerMiddlewareOptions>? configure = null)
    {
        var options = new ExceptionHandlerMiddlewareOptions();
        configure?.Invoke(options);
        return app.UseMiddleware<ExceptionHandlerMiddleware>(options);
    }
}