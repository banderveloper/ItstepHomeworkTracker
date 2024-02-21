using System.Security.Authentication;
using ItstepHomeworkTracker.Library.BySelectors;
using ItstepHomeworkTracker.Library.Exceptions;
using ItstepHomeworkTracker.Library.Extensions;
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
        
        // todo
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
        var errorSpan = _driver.FindElementOrNull(AuthorizationPageBySelectors.AuthorizationErrorSpan, timeoutInSeconds: 5);

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

        // If opened new homeworks popup - close it
        var popupCloseButton = _driver.FindElement(HomeworksPageBySelectors.NewHomeworksPopupCloseButton);
        popupCloseButton?.Click();
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
}
