using OpenQA.Selenium;

namespace ItstepHomeworkTracker.Library.BySelectors;

/// <summary>
/// Selenium By selectors for authorization page
/// </summary>
internal static class AuthorizationPageBySelectors
{
    /// <summary>
    /// Login input
    /// </summary>
    public static readonly By LoginInput = By.Id("login");

    /// <summary>
    /// Password input
    /// </summary>
    public static readonly By PasswordInput = By.Id("password");

    /// <summary>
    /// Submit button
    /// </summary>
    public static readonly By SubmitButton = By.ClassName("btn-login");
}
