using System.Text.Json.Serialization;

namespace ItstepHomeworkTracker.Library.Entities;

internal class StudentHomeworks
{
    [JsonPropertyName("studentName")]
    public string StudentName { get; set; }

    [JsonPropertyName("homeworks")]
    public IList<Homework> Homeworks { get; set; }

    public StudentHomeworks()
    {
        StudentName = string.Empty;
        Homeworks = new List<Homework>();
    }
}
