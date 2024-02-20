namespace ItstepHomeworkTracker.Library.Exceptions;

/// <summary>
/// Exception, thrown after failed authorization
/// </summary>
internal class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message) {}
}
