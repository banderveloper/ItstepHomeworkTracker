using OpenQA.Selenium;

namespace ItstepHomeworkTracker.BySelectors;

internal static class AuthorizationBySelectors
{
    public static readonly By LoginInput = By.Id("login");
    public static readonly By PasswordInput = By.Id("password");
    public static readonly By SubmitButton = By.ClassName("btn-login");
};