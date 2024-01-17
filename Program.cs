using ItstepHomeworkTracker;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

const string LOGBOOK_LOGIN = "Kalnicki_Nikita";
const string LOGBOOK_PASSWORD = "BpZ4bjS5871X";
const string GROUP_NAME = "3312";
const int TOTAL_HOMEWORKS_COUNT = 11;

var driver = new ChromeDriver();

driver.Url = "https://logbook.itstep.org/login/index#/";

driver.WaitAndFindElement(By.Id("login"));

var loginInput = driver.FindElement(By.Id("login"));
var passwordInput = driver.FindElement(By.Id("password"));

var submitButton = driver.FindElement(By.ClassName("btn-login"));

loginInput.SendKeys(LOGBOOK_LOGIN);
passwordInput.SendKeys(LOGBOOK_PASSWORD);

submitButton.Click();

driver.WaitAndFindElement(By.ClassName("open-menu-block"));
Console.WriteLine("Found menu block, start waiting for disappearing loader");
driver.WaitElementDisappear(By.ClassName("loader"));
Console.WriteLine("Loader disappeared");

driver.Url = "https://logbook.itstep.org/#/homeWork";

driver.WaitElementDisappear(By.ClassName("loader"));

if (driver.ElementVisible(By.ClassName("hw-md__close")))
    driver.FindElement(By.ClassName("hw-md__close")).Click();

driver.FindElement(By.CssSelector("md-select[ng-model='filter.group']")).Click();

var groupLink = driver.FindElement(By.XPath($"//md-option[text()='{GROUP_NAME}' and @ng-value='value.id_tgroups']"));
var id = groupLink.GetAttribute("id");

Thread.Sleep(1000);

driver.ExecuteScript($"document.getElementById('{id}').click()");

Thread.Sleep(1000);

IEnumerable<IWebElement> studentTrs = driver.FindElements(By.CssSelector("tr[ng-repeat='stud in stud_list']"));
Console.WriteLine(studentTrs.Count());

var statisticsList = new List<StudentHomeworkStatistics>();

foreach (var trElement in studentTrs)
{
    var studentName = trElement.FindElement(By.CssSelector("td[class='student-name'] p")).Text;

    var homeworkElements = trElement.FindElements(By.ClassName("hw_selects"));

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
    driver.FindElement(By.CssSelector("button[ng-disabled='endPosition <= minDays']")).Click();
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

