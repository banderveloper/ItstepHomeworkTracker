using OpenQA.Selenium;

namespace ItstepHomeworkTracker.Library.BySelectors;

/// <summary>
/// By selectors for homeworks page
/// </summary>
internal static class HomeworksPageBySelectors
{
    /// <summary>
    /// Close button for popup with new homeworks, opened in start of homeworks page
    /// </summary>
    public static readonly By NewHomeworksPopupCloseButton = By.ClassName("hw-md__close");
    
    /// <summary>
    /// Dropdown list with all groups
    /// </summary>
    public static readonly By GroupsListDropdownMenu = By.CssSelector("md-select[ng-model='filter.group']");
    
    /// <summary>
    /// Row with homeworks elements
    /// </summary>
    public static readonly By StudentHomeworksRow = By.CssSelector("tr[ng-repeat='stud in stud_list']");

    /// <summary>
    /// Next homeworks page button
    /// </summary>
    public static readonly By HomeworksNextPageButton = By.CssSelector("button[ng-disabled='endPosition <= minDays']");

    /// <summary>
    /// Get dropdown list with groups element by group name 
    /// </summary>
    /// <param name="groupName">Target group name</param>
    /// <returns>Configured selenium By selector</returns>
    public static By GetGroupLinkDropdownElement(string groupName) =>
        By.XPath($"//md-option[text()='{groupName}' and @ng-value='value.id_tgroups']");
}