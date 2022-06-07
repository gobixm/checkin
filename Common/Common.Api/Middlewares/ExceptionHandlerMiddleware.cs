using System.Text.Json;
using System.Text.Json.Serialization;
using Checkin.Common.Api.Models;
using Checkin.Common.Exceptions;
using Checkin.Common.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Checkin.Common.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly IHostEnvironment _environment;
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    private readonly ExceptionHandlerMiddlewareOptions _options;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger,
        IHostEnvironment environment, ExceptionHandlerMiddlewareOptions options)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (int statusCode, AppError dto) error = exception switch
        {
            CheckinException e => (e.StatusCode,
                new AppError(e.StatusCode.ToString(), e.Message, GetDetails(e), _options.ServiceName)),
            _ => (500,
                new AppError("500", "Unexpected error. Please contact your administrator.", GetDetails(exception),
                    _options.ServiceName))
        };

        context.Response.StatusCode = error.statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(error.dto, _jsonSerializerOptions);
    }

    private ErrorDetails? GetDetails(Exception ex)
    {
        if (_environment.IsProduction())
            return null;

        return new ErrorDetails(ex.Message, ex.StackTrace,
            ex.InnerException is not null ? GetDetails(ex.InnerException) : null);
    }
}