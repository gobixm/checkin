namespace Checkin.Common.Exceptions;

public class CheckinException : Exception
{
    private CheckinException(string message, int statusCode, Exception? inner)
        : base(message, inner)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }

    public static CheckinException Create(string message, int statusCode = 500, Exception? inner = null)
    {
        return new CheckinException(message, statusCode, inner);
    }

    public static void Throw(string message, int statusCode = 500, Exception? inner = null)
    {
        throw Create(message, statusCode, inner);
    }
}