namespace Checkin.Common.Exceptions;

public static class ExceptionExtensions
{
    public static CheckinException WrapToCheckinException(this Exception exception, string message, int statusCode = 500)
    {
        return CheckinException.Create(message, statusCode, exception);
    }
}