using CsvHelper.Configuration.Attributes;

namespace ItstepHomeworkTracker.Models;

public class StudentHomeworkStatistics
{
    [Name("Прізвище")]
    public string StudentName { get; set; }
    
    [Name("Сдано ДЗ")]
    public int CompletedHomeworksCount { get; set; }
    
    [Name("Усього ДЗ")]
    public int TotalHomeworksCount { get; set; }
    
    [Name("% сданих ДЗ")]
    public int CompletedHomeworksPercent => CompletedHomeworksCount * 100 / TotalHomeworksCount;
}