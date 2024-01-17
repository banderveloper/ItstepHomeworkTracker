using ItstepHomeworkTracker;
using ItstepHomeworkTracker.BySelectors;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

const string LOGBOOK_LOGIN = "Kalnicki_Nikita";
const string LOGBOOK_PASSWORD = "BpZ4bjS5871X";
const string GROUP_NAME = "3312";
const int TOTAL_HOMEWORKS_COUNT = 11;

var driver = new ChromeDriver();

driver.Url = "https://logbook.itstep.org/login/index#/";

driver.WaitAndFindElement(AuthorizationBySelectors.LoginInput);

var loginInput = driver.FindElement(AuthorizationBySelectors.LoginInput);
var passwordInput = driver.FindElement(AuthorizationBySelectors.PasswordInput);

var submitButton = driver.FindElement(AuthorizationBySelectors.SubmitButton);

loginInput.SendKeys(LOGBOOK_LOGIN);
passwordInput.SendKeys(LOGBOOK_PASSWORD);

submitButton.Click();

driver.WaitAndFindElement(CommonBySelectors.LeftMenu);
Console.WriteLine("Found menu block, start waiting for disappearing loader");
driver.WaitElementDisappear(CommonBySelectors.BoxLoader);
Console.WriteLine("Loader disappeared");

driver.Url = "https://logbook.itstep.org/#/homeWork";

driver.WaitElementDisappear(CommonBySelectors.BoxLoader);

if (driver.ElementVisible(HomeworksBySelectors.NewHomeworksPopupCloseButton))
    driver.FindElement(HomeworksBySelectors.NewHomeworksPopupCloseButton).Click();

driver.FindElement(HomeworksBySelectors.NewHomeworksPopupCloseButton).Click();

var groupLink = driver.FindElement(By.XPath($"//md-option[text()='{GROUP_NAME}' and @ng-value='value.id_tgroups']"));
var id = groupLink.GetAttribute("id");

Thread.Sleep(1000);

driver.ExecuteScript($"document.getElementById('{id}').click()");

Thread.Sleep(1000);

IEnumerable<IWebElement> studentTrs = driver.FindElements(HomeworksBySelectors.StudentHomeworksRow);
Console.WriteLine(studentTrs.Count());

var statisticsList = new List<StudentHomeworkStatistics>();

foreach (var trElement in studentTrs)
{
    var studentName = trElement.FindElement(HomeworksBySelectors.StudentNameInHomeworksRow).Text;

    var homeworkElements = trElement.FindElements(HomeworksBySelectors.HomeworkItemInHomeworksRow);

    int checkedHomeworksCount = homeworkElements.Count(element => element.FindElements(By.ClassName("hw_checked")).Count > 0);
    int notCheckedHomeworksCount = homeworkElements.Count(element => element.FindElements(By.ClassName("hw_new")).Count > 0);

    Console.WriteLine($"{studentName} -> {checkedHomeworksCount + notCheckedHomeworksCount}");

    statisticsList.Add(new StudentHomeworkStatistics
    {
        StudentName = studentName,
        CompletedHomeworksCount = checkedHomeworksCount + notCheckedHomeworksCount,
        TotalHomeworksCount = TOTAL_HOMEWORKS_COUNT
    });
}

if (TOTAL_HOMEWORKS_COUNT > 7)
{
    driver.FindElement(HomeworksBySelectors.HomeworksNextPageButton).Click();
    Thread.Sleep(100);
}

studentTrs = driver.FindElements(By.CssSelector("tr[ng-repeat='stud in stud_list']"));

foreach (var trElement in studentTrs)
{
    var studentName = trElement.FindElement(By.CssSelector("td[class='student-name'] p")).Text;

    var homeworkElements = trElement.FindElements(By.ClassName("hw_selects"));

    var checkedHomeworksCount = homeworkElements.Count(element => element.FindElements(By.ClassName("hw_checked")).Count > 0);
    var notCheckedHomeworksCount = homeworkElements.Count(element => element.FindElements(By.ClassName("hw_new")).Count > 0);
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

