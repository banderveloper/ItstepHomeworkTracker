using System.Text.Json;
using ItstepHomeworkTracker.BySelectors;
using ItstepHomeworkTracker.Extensions;
using ItstepHomeworkTracker.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ItstepHomeworkTracker;

public class LogbookWebDriver
{
    private readonly IWebDriver _driver = new ChromeDriver();

    public string AccountUsername { get; init; }
    public string AccountPassword { get; init; }
    public string TargetGroupName { get; init; }
    public int TotalHomeworksCount { get; init; }
    public int RequiredHomeworksPercent { get; init; }
    public event LogbookWebDriverDelegate? OnLogMessageSent;
    public event LogbookWebDriverDelegate? OnFinished;
    public event LogbookWebDriverDelegate? OnErrorMessageSent;


    public void Start()
    {
        OnLogMessageSent?.Invoke(this, new LogbookEventArgs("Start! Waiting for logbook loading..."));

        // go to auth page
        _driver.Url = "https://logbook.itstep.org/login/index#/";

        Authorize();

        // wait for home page loading
        _driver.WaitAndFindElement(CommonBySelectors.LeftMenu);
        WaitBoxLoading();

        OnLogMessageSent?.Invoke(this, new LogbookEventArgs("Navigating to homeworks page..."));

        // go to homeworks page and wait for loading
        _driver.Url = "https://logbook.itstep.org/#/homeWork";
        WaitBoxLoading();

        // close homeworks popup window if its displayed
        var closeButtonSelector = HomeworksBySelectors.NewHomeworksPopupCloseButton;
        if (_driver.ElementVisible(closeButtonSelector))
        {
            _driver.FindElement(closeButtonSelector).Click();
            OnLogMessageSent?.Invoke(this, new LogbookEventArgs("Closed new homeworks popup window."));
        }

        // click groups dropdown menu
        _driver.FindElement(HomeworksBySelectors.GroupsListDropdownMenu).Click();

        // find and click needed group in dropdown menu
        try
        {
            var groupLink = _driver.FindElement(HomeworksBySelectors.GetGroupLinkDropdownElement(TargetGroupName));
            var linkElementId = groupLink.GetAttribute("id");
            (_driver as ChromeDriver)!.ExecuteScript($"document.getElementById('{linkElementId}').click()");
        }
        catch (Exception ex)
        {
            OnErrorMessageSent?.Invoke(this, new LogbookEventArgs($"Group with name '{TargetGroupName}' not found."));
            return;
        }

        OnLogMessageSent?.Invoke(this, new LogbookEventArgs($"Chose group {TargetGroupName} in dropdown menu"));

        // sleep 1s
        Thread.Sleep(1000);

        OnLogMessageSent?.Invoke(this, new LogbookEventArgs("Waiting for homeworks loading..."));

        WaitBoxLoading();

        // dictionary <student name, completed homeworks count>
        var studentsHomeworks = new List<StudentHomeworks>();


        // calculate pagination data
        var homeworksPerPage = GetHomeworksCountInPage();
        var totalPagesCount = 0;

        OnLogMessageSent?.Invoke(this, new LogbookEventArgs($"One page has {homeworksPerPage} homeworks"));

        if (homeworksPerPage == TotalHomeworksCount)
            totalPagesCount = 1;
        else
            totalPagesCount = (int)Math.Ceiling((double)TotalHomeworksCount / homeworksPerPage);

        OnLogMessageSent?.Invoke(this, new LogbookEventArgs($"I will check {totalPagesCount} page(s)"));

        // iterate over each page and collect data about completed homeworks
        for (var currentPage = 1; currentPage <= totalPagesCount; currentPage++)
        {
            OnLogMessageSent?.Invoke(this, new LogbookEventArgs($"Collecting data from page {currentPage}/{totalPagesCount}"));
            // get all student's homework rows
            var studentsHomeworksRows = _driver.FindElements(HomeworksBySelectors.StudentHomeworksRow);

            int studentIndex = 0;

            // iterate student's rows
            foreach (var studentHomeworksRow in studentsHomeworksRows)
            {
                // get student name
                var studentName = studentHomeworksRow.FindElement(HomeworkRowBySelectors.StudentNameElement).Text;

                // try initialize student in dictionary with 0 hws
                if (currentPage == 1)
                {
                    studentsHomeworks.Add(new StudentHomeworks { StudentName = studentName });
                }

                // get all homeworks
                IEnumerable<IWebElement> homeworksItems =
                    studentHomeworksRow.FindElements(HomeworkRowBySelectors.HomeworkItem);

                // if last page - dont take all homeworks, only needed
                if (currentPage == totalPagesCount && totalPagesCount > 1)
                {
                    int leftHomeworksCount = TotalHomeworksCount % homeworksPerPage;
                    if (leftHomeworksCount == 0) leftHomeworksCount = homeworksPerPage;

                    homeworksItems = homeworksItems.Take(leftHomeworksCount);
                }

                foreach (var homeworkItem in homeworksItems)
                {
                    //studentsHomeworks[studentIndex].HomeworksCompleting
                    //    .Add(HomeworkItemNewOrCompleted(homeworkItem));
                    var homework = new Homework();
                    studentsHomeworks[studentIndex].Homeworks.Add(homework);

                    homework.IsCompleted = IsHomeworkCompleted(homeworkItem);

                    if (!homework.IsCompleted) continue;

                    homework.IsChecked = IsHomeworkChecked(homeworkItem);

                    if (!homework.IsChecked) continue;

                    homework.Grade = GetHomeworkGrade(homeworkItem);

                    if (homework.Grade <= 2) homework.IsCompleted = false;
                }

                // show
                studentIndex++;
            }

            if (currentPage < totalPagesCount) GoNextHomeworksPage();
        }

        OnLogMessageSent?.Invoke(this, new LogbookEventArgs($"Data collected, started export to {TargetGroupName}.html"));

        RenderHTMLStatistics(studentsHomeworks);

        OnFinished?.Invoke(this, new LogbookEventArgs("Done!", true));
    }

    private void Authorize()
    {
        OnLogMessageSent?.Invoke(this, new LogbookEventArgs("Authorizing..."));

        _driver.WaitAndFindElement(AuthorizationBySelectors.LoginInput);

        var loginInput = _driver.FindElement(AuthorizationBySelectors.LoginInput);
        var passwordInput = _driver.FindElement(AuthorizationBySelectors.PasswordInput);
        var submitButton = _driver.FindElement(AuthorizationBySelectors.SubmitButton);

        loginInput.SendKeys(AccountUsername);
        passwordInput.SendKeys(AccountPassword);

        submitButton.Click();

        OnLogMessageSent?.Invoke(this, new LogbookEventArgs("Submitted authorization, waiting for home page loading..."));
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

    private bool IsHomeworkChecked(IWebElement homeworkItem)
    {
        // try identify homework as new
        try
        {
            homeworkItem.FindElement(HomeworkRowBySelectors.NewHomeworkItem);
            return false;
        }
        catch (Exception)
        {
            return true;
        }
    }

    private bool IsHomeworkCompleted(IWebElement homeworkItem)
    {
        // try identify homework as new
        try
        {
            homeworkItem.FindElement(HomeworkRowBySelectors.UnloadedHomeworkItem);
            return false;
        }
        catch (Exception)
        {
            return true;
        }
    }

    private int GetHomeworkGrade(IWebElement homeworkItem)
    {
        var gradeElement = homeworkItem.FindElement(By.CssSelector("span"));
        return int.Parse(gradeElement.Text);
    }

    private void GoNextHomeworksPage()
    {
        _driver.FindElement(HomeworksBySelectors.HomeworksNextPageButton).Click();
    }

    private void RenderHTMLStatistics(List<StudentHomeworks> statisticsList)
    {
        string templateCode = string.Empty;

        using (var reader = new StreamReader("./template/template.html"))
        {
            templateCode = reader.ReadToEnd();
        }

        //File.WriteAllText("data.json", JsonSerializer.Serialize(statisticsList));

        templateCode = templateCode.Replace("%%%array_data%%%", JsonSerializer.Serialize(statisticsList));
        templateCode = templateCode.Replace("%%%total_homeworks_count%%%", TotalHomeworksCount.ToString());
        templateCode = templateCode.Replace("%%%requiredCompletePercent%%%", RequiredHomeworksPercent.ToString());
        templateCode = templateCode.Replace("%%%groupName%%%", TargetGroupName);

        using (var writer = new StreamWriter("./results/" + TargetGroupName + ".html"))
        {
            writer.Write(templateCode);
        }
    }

    public delegate void LogbookWebDriverDelegate(object sender, LogbookEventArgs e);
}