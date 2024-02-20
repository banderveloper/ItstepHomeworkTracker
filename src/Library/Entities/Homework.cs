using System.Text.Json.Serialization;

namespace ItstepHomeworkTracker.Library.Entities;

internal class Homework
{
    [JsonPropertyName("isCompleted")]
    public bool IsCompleted { get; set; }
    [JsonPropertyName("grade")]
    public int? Grade { get; set; }
}
