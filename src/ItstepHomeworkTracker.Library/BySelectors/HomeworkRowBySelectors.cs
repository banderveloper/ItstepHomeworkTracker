using OpenQA.Selenium;

namespace ItstepHomeworkTracker.Library.BySelectors;

/// <summary>
/// By selectors for homeworks row
/// </summary>
internal static class HomeworkRowBySelectors
{
    /// <summary>
    /// Student name element
    /// </summary>
    public static By StudentNameElement = By.CssSelector("td[class='student-name'] p");
    
    /// <summary>
    /// Top-level homework element
    /// </summary>
    public static By HomeworkItem = By.ClassName("hw_selects");
}