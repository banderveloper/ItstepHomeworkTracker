using System.Text.Json.Serialization;

namespace ItstepHomeworkTracker.Models;

public class StudentHomeworks
{
    [JsonPropertyName("name")]
    public string? StudentName { get; set; }

    [JsonPropertyName("homeworks")]
    public List<Homework> Homeworks { get; set; } = new();
}