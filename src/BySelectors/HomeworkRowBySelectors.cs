using OpenQA.Selenium;

namespace ItstepHomeworkTracker.BySelectors;

internal static class HomeworkRowBySelectors
{
    public static By StudentNameElement = By.CssSelector("td[class='student-name'] p");
    public static By HomeworkItem = By.ClassName("hw_selects");

    public static By CompletedHomeworkItem = By.ClassName("hw_checked");
    public static By NewHomeworkItem = By.ClassName("hw_new");
    public static By UnloadedHomeworkItem = By.ClassName("hw_not-loaded");
}