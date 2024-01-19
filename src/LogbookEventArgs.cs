namespace ItstepHomeworkTracker;

public class LogbookEventArgs : EventArgs
{
    public string LastLogMessage { get; }
    public bool IsFinished { get; }

    public LogbookEventArgs(string lastLogMessage, bool isFinished = false)
    {
        LastLogMessage = lastLogMessage;
        IsFinished = isFinished;
    }
}