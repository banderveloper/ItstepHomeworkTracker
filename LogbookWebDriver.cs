using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
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

        // click groups dropdown menu
        _driver.FindElement(HomeworksBySelectors.GroupsListDropdownMenu).Click();

        // find and click needed group in dropdown menu
        var groupLink = _driver.FindElement(HomeworksBySelectors.GetGroupLinkDropdownElement(_targetGroupName));
        var linkElementId = groupLink.GetAttribute("id");
        (_driver as ChromeDriver)!.ExecuteScript($"document.getElementById('{linkElementId}').click()");


        // sleep 1s
        Thread.Sleep(1000);

        WaitBoxLoading();

        // dictionary <student name, completed homeworks count>
        var completedHomeworksDictionary = new Dictionary<string, int>();

        // calculate pagination data
        var homeworksPerPage = GetHomeworksCountInPage();
        var totalPagesCount = 0;

        if (homeworksPerPage == _totalHomeworksCount)
            totalPagesCount = 1;
        else
            totalPagesCount = (int)Math.Ceiling((double)_totalHomeworksCount / homeworksPerPage);
        
        // iterate over each page and collect data about completed homeworks
        for (var currentPage = 1; currentPage <= totalPagesCount; currentPage++)
        {
            Console.WriteLine($"==== PAGE {currentPage} ====");
            // get all student's homework rows
            var studentsHomeworksRows = _driver.FindElements(HomeworksBySelectors.StudentHomeworksRow);

            // iterate student's rows
            foreach (var studentHomeworksRow in studentsHomeworksRows)
            {
                // get student name
                var studentName = studentHomeworksRow.FindElement(HomeworkRowBySelectors.StudentNameElement).Text;

                // try initialize student in dictionary with 0 hws
                completedHomeworksDictionary.TryAdd(studentName, 0);

                // get all homeworks
                IEnumerable<IWebElement> homeworksItems =
                    studentHomeworksRow.FindElements(HomeworkRowBySelectors.HomeworkItem);

                // if last page - dont take all homeworks, only needed
                if (currentPage == totalPagesCount && totalPagesCount > 1)
                {
                    int leftHomeworksCount = _totalHomeworksCount % homeworksPerPage;
                    homeworksItems = homeworksItems.Take(leftHomeworksCount);
                }

                // get only completed/new homeworks count
                var completedHomeworksCount = homeworksItems.Count(HomeworkItemNewOrCompleted);

                // add new/completed homeworks count to student's statistics
                completedHomeworksDictionary[studentName] += completedHomeworksCount;

                // show
                Console.WriteLine($"{studentName} +{completedHomeworksCount}");
            }

            GoNextHomeworksPage();

            Console.WriteLine();
        }
        
        WriteDataToCsv(completedHomeworksDictionary);
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

    private int GetHomeworksCountInPage()
    {
        var firstRow = _driver.FindElement(HomeworksBySelectors.StudentHomeworksRow);
        return firstRow.FindElements(HomeworkRowBySelectors.HomeworkItem).Count;
    }

    private bool HomeworkItemNewOrCompleted(IWebElement homeworkItem)
    {
        // try identify homework as new
        try
        {
            homeworkItem.FindElement(HomeworkRowBySelectors.NewHomeworkItem);
            return true;
        }
        catch (Exception)
        {
            // ignored
        }

        // try identify homework as completed
        try
        {
            homeworkItem.FindElement(HomeworkRowBySelectors.CompletedHomeworkItem);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private void GoNextHomeworksPage()
    {
        _driver.FindElement(HomeworksBySelectors.HomeworksNextPageButton).Click();
    }

    private void WriteDataToCsv(Dictionary<string, int> completedHomeworksDictionary)
    {
        var statisticsList = new List<StudentHomeworkStatistics>();

        foreach (var pair in completedHomeworksDictionary)
        {
            var listItem = new StudentHomeworkStatistics
            {
                StudentName = pair.Key,
                CompletedHomeworksCount = pair.Value,
                TotalHomeworksCount = _totalHomeworksCount
            };
            statisticsList.Add(listItem);
        }

        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
            Encoding = Encoding.UTF8
        };

        using var streamWriter = new StreamWriter("homeworks.csv");
        using var csvWriter = new CsvWriter(streamWriter, csvConfiguration);
        
        csvWriter.WriteRecords(statisticsList);
    }
}