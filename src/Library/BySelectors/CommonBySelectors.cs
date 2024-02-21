using OpenQA.Selenium;

namespace ItstepHomeworkTracker.Library.BySelectors;

/// <summary>
/// By selector, used in a few pages
/// </summary>
internal static class CommonBySelectors
{
    /// <summary>
    /// Default logbook box loader
    /// </summary>
    public static readonly By BoxLoader = By.ClassName("loader");
    
    /// <summary>
    /// Left navigation menu
    /// </summary>
    public static readonly By LeftMenu = By.ClassName("open-menu-block");
};