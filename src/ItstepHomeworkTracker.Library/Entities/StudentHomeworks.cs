using System.Text.Json.Serialization;

namespace ItstepHomeworkTracker.Library.Entities;

/// <summary>
/// Binding student name to list of his homeworks
/// </summary>
internal class StudentHomeworks
{
    /// <summary>
    /// Full name of student
    /// </summary>
    [JsonPropertyName("studentName")]
    public string StudentName { get; set; }
    
    /// <summary>
    /// List of all student's homework
    /// </summary>
    [JsonPropertyName("homeworks")]
    public ICollection<Homework> Homeworks { get; set; }

    public StudentHomeworks()
    {
        StudentName = string.Empty;
        Homeworks = new List<Homework>();
    }

    public StudentHomeworks(string studentName)
    {
        StudentName = studentName;
        Homeworks = new List<Homework>();
    }
}
