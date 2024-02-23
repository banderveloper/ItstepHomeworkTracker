using System.Security.Authentication;
using System.Text.Json;
using ItstepHomeworkTracker.Library.BySelectors;
using ItstepHomeworkTracker.Library.Entities;
using ItstepHomeworkTracker.Library.Exceptions;
using ItstepHomeworkTracker.Library.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TimeoutException = ItstepHomeworkTracker.Library.Exceptions.TimeoutException;

namespace ItstepHomeworkTracker.Library;

/// <summary>
/// Parser core, wrapper for selenium web driver
/// </summary>
internal class LogbookWebDriver
{
    /// <summary>
    /// Selenium driver
    /// </summary>
    private readonly ChromeDriver _driver = new();

    /// <summary>
    /// Timeout for waiting page loading / element searching
    /// </summary>
    private readonly int _timeoutInSeconds = 10;

    /// <summary>
    /// Logbook username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Logbook password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Group name to track homeworks
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// Count of given homeworks
    /// </summary>
    public int TotalHomeworksCount { get; set; }

    /// <summary>
    /// Required percent of completed homeworks to be passed to exam
    /// </summary>
    public int RequiredHomeworksPercent { get; set; }

    /// <summary>
    /// Start parsing process
    /// </summary>
    public void Start()
    {
        Authorize();
        ProceedToHomeworkPage();
        SelectGroupHomeworksTab();

        var homeworksPerPage = GetHomeworksCountPerPage();
        var pagesCount = GetTotalPagesCount(homeworksPerPage);

        var studentsHomeworksList = GetAllStudentsHomeworks(pagesCount, homeworksPerPage);
        RenderHTMLResult(studentsHomeworksList);
    }

    /// <summary>
    /// Proceed authorization page
    /// </summary>
    /// <exception cref="TimeoutException">Page is not loaded after timeout</exception>
    /// <exception cref="AuthenticationException">Authentication failed</exception>
    private void Authorize()
    {
        // Go to any logbook page and get to auth page if unauthorized
        _driver.Url = "https://logbook.itstep.org/login/index#/";

        // Wait for login input loading and find it
        var loginInput = _driver.FindElementOrNull(AuthorizationPageBySelectors.LoginInput, _timeoutInSeconds);

        // If login input not found after seconds of timeout - exception
        if (loginInput is null)
            throw new TimeoutException($"Login input not found after {_timeoutInSeconds} seconds");

        // We know that page is loaded, without timeout find password input and submit button
        var passwordInput = _driver.FindElement(AuthorizationPageBySelectors.PasswordInput);
        var submitButton = _driver.FindElement(AuthorizationPageBySelectors.SubmitButton);

        // Fill inputs with credentials
        loginInput.SendKeys(Username);
        passwordInput.SendKeys(Password);

        // Submit
        submitButton.Click();

        // Wait 3 seconds for auth error span
        var errorSpan =
            _driver.FindElementOrNull(AuthorizationPageBySelectors.AuthorizationErrorSpan, timeoutInSeconds: 5);

        // If auth error exists - auth error
        if (errorSpan is not null)
            throw new AuthenticationException("Authentication error. " + errorSpan.Text);
    }

    /// <summary>
    /// Go to homeworks page and close potential popup form
    /// </summary>
    private void ProceedToHomeworkPage()
    {
        // Wait for page rendering and wait for box loading
        _driver.FindElementOrNull(CommonBySelectors.BoxLoader, _timeoutInSeconds);
        _driver.WaitForElementHide(CommonBySelectors.BoxLoader);

        // go to homeworks page
        _driver.Url = "https://logbook.itstep.org/#/homeWork";

        // Wait for page rendering and wait for box loading
        _driver.FindElementOrNull(CommonBySelectors.BoxLoader, _timeoutInSeconds);
        _driver.WaitForElementHide(CommonBySelectors.BoxLoader);

        // Get potential popup window with new homeworks
        var popupCloseButton = _driver.FindElementOrNull(HomeworksPageBySelectors.NewHomeworksPopupCloseButton);
        
        // If popup opened - close it
        if (popupCloseButton is { Enabled: true, Displayed: true })
            popupCloseButton.Click();
    }

    /// <summary>
    /// Open groups dropdown menu and select needed, invoking update of homeworks list 
    /// </summary>
    private void SelectGroupHomeworksTab()
    {
        // Click on groups list dropdown menu
        _driver.FindElement(HomeworksPageBySelectors.GroupsListDropdownMenu).Click();

        // Find link with needed group in dropdown menu
        var groupLinkElement =
            _driver.FindElementOrNull(HomeworksPageBySelectors.GetGroupLinkDropdownElement(GroupName));

        // If group with name not found
        if (groupLinkElement is null)
            throw new GroupNotFoundException($"Group '{GroupName}' not found");

        // Element is not clickable by selenium, get element id and click using javascript
        var linkElementId = groupLinkElement.GetAttribute("id");
        _driver.ExecuteScript($"document.getElementById('{linkElementId}').click()");

        // Wait for box loading
        _driver.FindElementOrNull(CommonBySelectors.BoxLoader, _timeoutInSeconds);
        _driver.WaitForElementHide(CommonBySelectors.BoxLoader);
    }

    /// <summary>
    /// Get homeworks count in one tab
    /// </summary>
    /// <returns>Count of homeworks per page</returns>
    private int GetHomeworksCountPerPage()
    {
        // Select first row of homeworks to calculate homeworks per page
        var firstHomeworksRow = _driver.FindElement(HomeworksPageBySelectors.StudentHomeworksRow);
        return firstHomeworksRow.FindElements(HomeworkRowBySelectors.HomeworkItem).Count;
    }

    /// <summary>
    /// Get amount of pages, needed to be parsed for full data collection
    /// </summary>
    /// <param name="homeworksPerPage">Homeworks count per one page</param>
    /// <returns>Count of pages, needed to be checked</returns>
    private int GetTotalPagesCount(int homeworksPerPage)
    {
        if (homeworksPerPage == TotalHomeworksCount)
            return 1;

        return (int)Math.Ceiling((double)TotalHomeworksCount / homeworksPerPage);
    }

    /// <summary>
    /// Get homeworks elements from row. Pagination data is needed to cut needed homeworks in the last page
    /// </summary>
    /// <param name="homeworksRow">Homeworks row element</param>
    /// <param name="page">Current homeworks page</param>
    /// <param name="pagesCount">Total pages count</param>
    /// <param name="homeworksPerPage">Homeworks elements per page</param>
    /// <returns>IEnumerable of homeworks elements</returns>
    private IEnumerable<IWebElement> GetHomeworksElementsFromRow(IWebElement homeworksRow, int page, int pagesCount,
        int homeworksPerPage)
    {
        // Get all homeworks elements from row
        IEnumerable<IWebElement> homeworksItems = homeworksRow.FindElements(HomeworkRowBySelectors.HomeworkItem);

        // If not last page - just return all homeworks
        if (page != pagesCount || pagesCount <= 1) return homeworksItems;

        // If page is last - we need to take only left count of homeworks 
        var leftHomeworksCount = TotalHomeworksCount % homeworksPerPage;
        if (leftHomeworksCount == 0) leftHomeworksCount = homeworksPerPage;

        homeworksItems = homeworksItems.Take(leftHomeworksCount);

        return homeworksItems;
    }

    /// <summary>
    /// Parse and take homework data from homework element
    /// </summary>
    /// <param name="homeworkElement">Target homework element</param>
    /// <returns>Homework data</returns>
    /// <exception cref="Exception">If found unknown element class</exception>
    private Homework GetHomeworkDataFromElement(IWebElement homeworkElement)
    {
        var result = new Homework();

        // Needed class is in deep button
        var homeworkButtonElement = homeworkElement.FindElement(By.TagName("button"));

        // Needed class is always second in class list
        var className = homeworkButtonElement.GetAttribute("class").Split(" ")[1];

        switch (className)
        {
            // homework not loaded or overdue
            case "hw_not-loaded":
            case "hw_overdue":
                result.IsCompleted = false;
                break;

            // homework loaded but teacher doesn't checked it
            // or homework is on recheck
            case "hw_new":
            case "hw_retake":
                result.IsCompleted = true;
                break;

            // homework loaded and teacher checked it
            case "hw_checked":
                result.IsCompleted = true;
                // take grade from element
                var gradeElement = homeworkElement.FindElement(By.TagName("span"));
                result.Grade = int.Parse(gradeElement.Text);
                break;

            // if unknown class
            default:
                throw new Exception("Unknown homework type");
        }

        return result;
    }

    /// <summary>
    /// Parse homeworks pages and take homeworks data
    /// </summary>
    /// <param name="pagesCount">Total count of pages, needed to be parsed</param>
    /// <param name="homeworksPerPage">Homeworks elements per page</param>
    /// <returns>Collection of student names and their homeworks</returns>
    private ICollection<StudentHomeworks> GetAllStudentsHomeworks(int pagesCount, int homeworksPerPage)
    {
        var studentsHomeworksList = new List<StudentHomeworks>();

        // iterate over homeworks pages
        for (var page = 1; page <= pagesCount; page++)
        {
            var studentIndex = 0;
            // get all rows with homeworks
            var homeworksRows = _driver.FindElements(HomeworksPageBySelectors.StudentHomeworksRow);

            // iterate over homeworks rows
            foreach (var homeworkRow in homeworksRows)
            {
                // get student name from fow
                var studentName = homeworkRow.FindElement(HomeworkRowBySelectors.StudentNameElement).Text;

                // if it is first page - add new cell to list with student name
                if (page == 1)
                    studentsHomeworksList.Add(new StudentHomeworks(studentName));

                // get homeworks elements from row, optionally don't take all hws from last page
                var homeworksElements = GetHomeworksElementsFromRow(homeworkRow, page, pagesCount, homeworksPerPage);

                // iterate over homeworks elements
                foreach (var homeworkElement in homeworksElements)
                {
                    // parse homework and add data to result list
                    var homework = GetHomeworkDataFromElement(homeworkElement);
                    studentsHomeworksList[studentIndex].Homeworks.Add(homework);
                }

                studentIndex++;
            }

            // Go to next page until all pages are checked
            if (page < pagesCount) ProceedNextHomeworksPage();
        }

        return studentsHomeworksList;
    }

    /// <summary>
    /// Go to next homeworks page
    /// </summary>
    private void ProceedNextHomeworksPage()
    {
        _driver.FindElementOrNull(HomeworksPageBySelectors.HomeworksNextPageButton)?.Click();
    }

    /// <summary>
    /// Render final html result with homeworks statistics
    /// </summary>
    /// <param name="studentsHomeworks">Collection of student names and their homeworks</param>
    private void RenderHTMLResult(ICollection<StudentHomeworks> studentsHomeworks)
    {
        string templateCode;

        using (var reader = new StreamReader("./template.html"))
        {
            templateCode = reader.ReadToEnd();
        }

        //File.WriteAllText("data.json", JsonSerializer.Serialize(statisticsList));

        templateCode = templateCode.Replace("%%%array_data%%%", JsonSerializer.Serialize(studentsHomeworks));
        templateCode = templateCode.Replace("%%%total_homeworks_count%%%", TotalHomeworksCount.ToString());
        templateCode = templateCode.Replace("%%%requiredCompletePercent%%%", RequiredHomeworksPercent.ToString());
        templateCode = templateCode.Replace("%%%groupName%%%", GroupName);

        using (var writer = new StreamWriter("./results/" + GroupName + ".html"))
        {
            writer.Write(templateCode);
        }
    }
}