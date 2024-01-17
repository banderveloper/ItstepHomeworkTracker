using OpenQA.Selenium;

namespace ItstepHomeworkTracker.BySelectors;

public static class HomeworksBySelectors
{
    public static By NewHomeworksPopupCloseButton = By.ClassName("hw-md__close");
    public static By GroupsListDropdownMenu = By.CssSelector("md-select[ng-model='filter.group']");
    public static By StudentHomeworksRow = By.CssSelector("tr[ng-repeat='stud in stud_list']");
    public static By StudentNameInHomeworksRow = By.CssSelector("td[class='student-name'] p");
    public static By HomeworkItemInHomeworksRow = By.ClassName("hw_selects");

    

    public static By HomeworksNextPageButton = By.CssSelector("button[ng-disabled='endPosition <= minDays']");
}