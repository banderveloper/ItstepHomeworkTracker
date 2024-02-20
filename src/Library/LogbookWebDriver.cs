using ItstepHomeworkTracker.Library.BySelectors;
using ItstepHomeworkTracker.Library.Exceptions;
using ItstepHomeworkTracker.Library.Extensions;
using OpenQA.Selenium.Chrome;

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
        var loginInput = _driver.FindElementContinuously(AuthorizationPageBySelectors.LoginInput, _timeoutInSeconds);

        // We know that page is loaded, without timeout find password input and submit button
        var passwordInput = _driver.FindElement(AuthorizationPageBySelectors.PasswordInput);
        var submitButton = _driver.FindElement(AuthorizationPageBySelectors.SubmitButton);

        // Fill inputs with credentials
        loginInput.SendKeys(Username);
        passwordInput.SendKeys(Password);

        submitButton.Click();
        
        // TODO: wait for incorrect credentials error
        var errorSpan = _driver.FindElementContinuously(AuthorizationPageBySelectors.AuthorizationErrorSpan, 3);

        // Submit
    }
}
