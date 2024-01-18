using ItstepHomeworkTracker.BySelectors;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ItstepHomeworkTracker;

public class LogbookWebDriver
{
    private readonly IWebDriver _driver;

    private readonly string _accountUsername;
    private readonly string _accountPassword;

    private readonly string _targetGroupName;
    private readonly int _totalHomeworksCount;

    public LogbookWebDriver(string accountUsername, string accountPassword, string targetGroupName,
        int totalHomeworksCount) =>
        (_accountUsername, _accountPassword, _targetGroupName, _totalHomeworksCount, _driver) =
        (accountUsername, accountPassword, targetGroupName, totalHomeworksCount, new ChromeDriver());

    public void Start()
    {
        // go to auth page
        _driver.Url = "https://logbook.itstep.org/login/index#/";

        Authorize();

        // wait for home page loading
        _driver.WaitAndFindElement(CommonBySelectors.LeftMenu);
        WaitBoxLoading();

        // go to homeworks page and wait for loading
        _driver.Url = "https://logbook.itstep.org/#/homeWork";
        WaitBoxLoading();

        // close homeworks popup window if its displayed
        var closeButtonSelector = HomeworksBySelectors.NewHomeworksPopupCloseButton;
        if (_driver.ElementVisible(closeButtonSelector))
            _driver.FindElement(closeButtonSelector).Click();
        
        Thread.Sleep(100);

        _driver.FindElement(HomeworksBySelectors.GroupsListDropdownMenu).Click();
        
        Thread.Sleep(100);
        
        // find and click needed group in dropdown menu
        var groupLink =
            _driver.FindElement(By.XPath($"//md-option[text()='{_targetGroupName}' and @ng-value='value.id_tgroups']"));
        var linkElementId = groupLink.GetAttribute("id");
        (_driver as ChromeDriver)!.ExecuteScript($"document.getElementById('{linkElementId}').click()");

        Thread.Sleep(1000);
        
        // 
        IEnumerable<IWebElement> studentTrs = _driver.FindElements(HomeworksBySelectors.StudentHomeworksRow);
        Console.WriteLine(studentTrs.Count());

        var statisticsList = new List<StudentHomeworkStatistics>();

        foreach (var trElement in studentTrs)
        {
            var studentName = trElement.FindElement(HomeworksBySelectors.StudentNameInHomeworksRow).Text;

            var homeworkElements = trElement.FindElements(HomeworksBySelectors.HomeworkItemInHomeworksRow);

            int checkedHomeworksCount =
                homeworkElements.Count(element => element.FindElements(By.ClassName("hw_checked")).Count > 0);
            int notCheckedHomeworksCount =
                homeworkElements.Count(element => element.FindElements(By.ClassName("hw_new")).Count > 0);

            Console.WriteLine($"{studentName} -> {checkedHomeworksCount + notCheckedHomeworksCount}");

            statisticsList.Add(new StudentHomeworkStatistics
            {
                StudentName = studentName,
                CompletedHomeworksCount = checkedHomeworksCount + notCheckedHomeworksCount,
                TotalHomeworksCount = _totalHomeworksCount
            });
        }

        if (_totalHomeworksCount > 7)
        {
            _driver.FindElement(HomeworksBySelectors.HomeworksNextPageButton).Click();
            Thread.Sleep(100);
        }

        studentTrs = _driver.FindElements(By.CssSelector("tr[ng-repeat='stud in stud_list']"));

        foreach (var trElement in studentTrs)
        {
            var studentName = trElement.FindElement(By.CssSelector("td[class='student-name'] p")).Text;

            var homeworkElements = trElement.FindElements(By.ClassName("hw_selects"));

            var checkedHomeworksCount =
                homeworkElements.Count(element => element.FindElements(By.ClassName("hw_checked")).Count > 0);
            var notCheckedHomeworksCount =
                homeworkElements.Count(element => element.FindElements(By.ClassName("hw_new")).Count > 0);
            var totalCompletedHomeworks = checkedHomeworksCount + notCheckedHomeworksCount;

            Console.WriteLine($"{studentName} -> {checkedHomeworksCount + notCheckedHomeworksCount}");

            statisticsList.FirstOrDefault(i => i.StudentName.Equals(studentName)).CompletedHomeworksCount +=
                totalCompletedHomeworks;
        }

        foreach (var studentInfo in statisticsList)
        {
            Console.WriteLine(studentInfo);
        }

        Console.ReadLine();
    }

    private void Authorize()
    {
        _driver.WaitAndFindElement(AuthorizationBySelectors.LoginInput);

        var loginInput = _driver.FindElement(AuthorizationBySelectors.LoginInput);
        var passwordInput = _driver.FindElement(AuthorizationBySelectors.PasswordInput);
        var submitButton = _driver.FindElement(AuthorizationBySelectors.SubmitButton);

        loginInput.SendKeys(_accountUsername);
        passwordInput.SendKeys(_accountPassword);

        submitButton.Click();
    }

    private void WaitBoxLoading()
    {
        _driver.WaitElementDisappear(CommonBySelectors.BoxLoader);
    }
}