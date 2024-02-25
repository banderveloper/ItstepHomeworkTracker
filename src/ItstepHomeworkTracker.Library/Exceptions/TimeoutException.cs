namespace ItstepHomeworkTracker.Library.Exceptions;

/// <summary>
/// Exception, thrown after finding time out
/// </summary>
internal class TimeoutException : Exception
{
    public TimeoutException(string message) : base(message) {}
}
