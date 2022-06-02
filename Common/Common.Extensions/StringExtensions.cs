namespace Checkin.Common.Extensions;

public static class StringExtensions
{
    public static string TrimLower(this string source)
    {
        return source.Trim().ToLowerInvariant();
    }
}