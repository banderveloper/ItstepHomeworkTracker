namespace ItstepHomeworkTracker;

public class StudentHomeworkStatistics
{
    public string StudentName { get; set; }
    public int CompletedHomeworksCount { get; set; }
    public int TotalHomeworksCount { get; set; }

    public double CompletedHomeworksPercent => CompletedHomeworksCount * 100 / (double)TotalHomeworksCount;
    public override string ToString() => $"{StudentName} | {CompletedHomeworksCount}/{TotalHomeworksCount} ({CompletedHomeworksPercent:0.##})";
}