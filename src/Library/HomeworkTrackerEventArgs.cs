namespace ItstepHomeworkTracker.Library;

public class HomeworkTrackerEventArgs: EventArgs
{
    public string Message { get; }

    public HomeworkTrackerEventArgs(string message) => Message = message;
}
