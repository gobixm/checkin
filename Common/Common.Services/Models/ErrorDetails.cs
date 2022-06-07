namespace Checkin.Common.Services.Models;

public sealed record ErrorDetails(string? Message, string? Stack, ErrorDetails? Inner);