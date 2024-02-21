using OpenQA.Selenium;

namespace ItstepHomeworkTracker.Library.BySelectors;

internal static class HomeworksPageBySelectors
{
    public static By NewHomeworksPopupCloseButton = By.ClassName("hw-md__close");
    public static By GroupsListDropdownMenu = By.CssSelector("md-select[ng-model='filter.group']");
    public static By StudentHomeworksRow = By.CssSelector("tr[ng-repeat='stud in stud_list']");
    
    public static By HomeworksNextPageButton = By.CssSelector("button[ng-disabled='endPosition <= minDays']");

    public static By GetGroupLinkDropdownElement(string groupName) =>
        By.XPath($"//md-option[text()='{groupName}' and @ng-value='value.id_tgroups']");
}