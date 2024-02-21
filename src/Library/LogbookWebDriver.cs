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
    private ChromeDriver _driver = new ChromeDriver();

    /// <summary>
    /// Timeout for waiting page loading / element searching
    /// </summary>
    private int _timeoutInSeconds = 50;

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
    }

    private void Authorize()
    {
        // Go to any logbook page and get to auth page if unauthorized
        _driver.Url = "https://logbook.itstep.org/login/index#/";

        // Wait for login input loading and find it
        var loginInput = _driver.FindElement(AuthorizationPageBySelectors.LoginInput, _timeoutInSeconds);

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
        var errorSpan = _driver.FindElement(AuthorizationPageBySelectors.AuthorizationErrorSpan, timeoutInSeconds: 5);

        // If auth error exists - auth error
        if (errorSpan is not null)
            throw new AuthenticationException("Authentication error. " + errorSpan.Text);
    }
}
