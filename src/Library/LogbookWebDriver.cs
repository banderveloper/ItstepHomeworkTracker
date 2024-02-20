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

    }

    private void Authorize()
    {
        // Go to any logbook page and get to auth page if unauthorized
        _driver.Url = "https://logbook.itstep.org/login/index#/";


    }
}
