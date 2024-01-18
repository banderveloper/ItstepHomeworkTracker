using OpenQA.Selenium;

namespace ItstepHomeworkTracker.Extensions;

public static class WebDriverExtensions
{
    private const int RepeatTimeout = 50;

    public static IWebElement WaitAndFindElement(this IWebDriver driver, By by)
    {
        while (true)
        {
            try
            {
                var element = driver.FindElement(by);
                return element;
            }
            catch (Exception)
            {
                // ignored
            }
            Thread.Sleep(RepeatTimeout);
        }
    }

    public static void WaitElementDisappear(this IWebDriver driver, By by)
    {
        while (true)
        {
            try
            {
                var element = driver.FindElement(by);
                if (!element.Displayed) return;
            }
            catch (Exception)
            {
                return;
            }
            Thread.Sleep(RepeatTimeout);
        }
    }

    public static bool ElementExists(this IWebDriver driver, By by)
    {
        try
        {
            driver.FindElement(by);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool ElementVisible(this IWebDriver driver, By by)
    {
        try
        {
            var element = driver.FindElement(by);
            return element.Displayed;
        }
        catch (Exception)
        {
            return false;
        }
    }
}