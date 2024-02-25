namespace ItstepHomeworkTracker.Library.Exceptions;

/// <summary>
/// Exception, thrown after group not found in homeworks tab
/// </summary>
internal class GroupNotFoundException : Exception
{
    public GroupNotFoundException(string message) : base(message) {}
}
