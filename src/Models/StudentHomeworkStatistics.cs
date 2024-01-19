using System.Text.Json.Serialization;

namespace ItstepHomeworkTracker.Models;

internal class StudentHomeworkStatistics
{
    [JsonPropertyName("name")]
    public string StudentName { get; set; }
    
    [JsonPropertyName("homeworks")]
    public List<bool> HomeworksCompleting { get; set; }

    public StudentHomeworkStatistics(string studentName)
    {
        StudentName = studentName;
        HomeworksCompleting = new List<bool>();
    }
}