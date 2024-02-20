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
    /// Synchronously wait for element during given timeout
    /// </summary>
    /// <param name="driver">Extension method target</param>
    /// <param name="by">Selenium by selector, target of finding</param>
    /// <param name="timeoutInSeconds">Timeout for finding</param>
    /// <returns>Found web element</returns>
    public static IWebElement FindElementContinuously(this ChromeDriver driver, By by, int timeoutInSeconds)
    {
        if (timeoutInSeconds > 0)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv => drv.FindElement(by));
        }
        return driver.FindElement(by);
    }
}
