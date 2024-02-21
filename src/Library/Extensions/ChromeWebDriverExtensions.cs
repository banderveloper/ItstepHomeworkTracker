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
    public static IWebElement? FindElementOrNull(this ChromeDriver driver, By by, int timeoutInSeconds = 0)
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

    /// <summary>
    /// Start loop until given element disapper
    /// </summary>
    /// <param name="driver">Extension method target</param>
    /// <param name="by">Selenium by selector, target of disappear waiting</param>
    /// <param name="timeoutInSeconds">Wait timeout, maximum by default</param>
    public static void WaitForElementDisappear(this ChromeDriver driver, By by, int timeoutInSeconds = int.MaxValue)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.Until(webDriver => webDriver.FindElements(by).Count == 0);
    }
    
    /// <summary>
    /// Start loop until given element hide
    /// </summary>
    /// <param name="driver">Extension method target</param>
    /// <param name="by">Selenium by selector, target of hide waiting</param>
    /// <param name="timeoutInSeconds">Wait timeout, maximum by default</param>
    public static void WaitForElementHide(this ChromeDriver driver, By by, int timeoutInSeconds = int.MaxValue)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.Until(webDriver => !webDriver.FindElement(by).Displayed);
    }
}