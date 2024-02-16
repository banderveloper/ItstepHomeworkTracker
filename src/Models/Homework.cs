using System.Text.Json.Serialization;

namespace ItstepHomeworkTracker.Models;

public class Homework
{
    [JsonPropertyName("is_completed")]
    public bool IsCompleted { get; set; }

    [JsonPropertyName("is_checked")]
    public bool IsChecked { get; set; }

    [JsonPropertyName("grade")]
    public int? Grade { get; set; }
}
