using System.Text.Json.Serialization;

namespace ItstepHomeworkTracker.Library.Entities;

/// <summary>
/// Homework from homeworks page, grade and completion status
/// </summary>
internal class Homework
{
    /// <summary>
    /// Is homework completed by student
    /// </summary>
    [JsonPropertyName("isCompleted")]
    public bool IsCompleted { get; set; }
    
    /// <summary>
    /// Grade, given by teacher (or null if not checked)
    /// </summary>
    [JsonPropertyName("grade")]
    public int? Grade { get; set; }
}
