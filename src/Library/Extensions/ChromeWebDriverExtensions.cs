using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ItstepHomeworkTracker.Library.Extensions;

/// <summary>
/// Extensions methods for selenium ChromeWebDriver
/// </summary>
internal static class ChromeWebDriverExtensions
{
    /// <summary>
    /// Decorator for default FindElement method, for optional timeout 
    /// </summary>
    /// <param name="driver">Extension method target</param>
    /// <param name="by">Selenium by selector, target of finding</param>
    /// <param name="timeoutInSeconds">Timeout for finding</param>
    /// <returns>Found web element, or null if not found</returns>
    public static IWebElement? FindElement(this ChromeDriver driver, By by, int timeoutInSeconds = 0)
    {
        try
        {
            // If no timeout - just find element
            if (timeoutInSeconds <= 0) return driver.FindElement(by);

            // if timeout - wait and find
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv => drv.FindElement(by));
        }
        catch (Exception)
        {
            return null;
        }
    }
}