namespace Checkin.Common.Services.Models;

public sealed record AppError(string Code, string? Message = null, ErrorDetails? Details = null, string? Service = null);