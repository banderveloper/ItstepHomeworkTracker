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

    /// <summary>
    /// Disappearing span after authorization error
    /// </summary>
    public static readonly By AuthorizationErrorSpan = By.ClassName("md-toast-text");
}
