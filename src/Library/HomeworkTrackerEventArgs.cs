namespace ItstepHomeworkTracker.Library;

/// <summary>
/// Event args for tracker events, such as OnParsingError
/// </summary>
public class HomeworkTrackerEventArgs: EventArgs
{
    public string Message { get; }

    public HomeworkTrackerEventArgs(string message) => Message = message;
}
