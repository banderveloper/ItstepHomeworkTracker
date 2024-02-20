namespace ItstepHomeworkTracker.Library;

/// <summary>
/// Public presentation of parser, library 'api'
/// </summary>
public class HomeworkTracker
{
    /// <summary>
    /// Parser core
    /// </summary>
    private LogbookWebDriver _logbookDriver;

    /// <summary>
    /// Event, invoked after parsing exception
    /// </summary>
    public event HomeworkTrackerDelegate? OnParsingError;

    public HomeworkTracker()
        => _logbookDriver = new LogbookWebDriver();

    /// <summary>
    /// Start parsing with exception waiting, invoke error event if ex
    /// </summary>
    public void Start()
    {
        try
        {
            _logbookDriver.Start();
        }
        catch (Exception ex)
        {
            OnParsingError?.Invoke(this, new HomeworkTrackerEventArgs(ex.Message));
        }
    }

    /// <summary>
    /// Builder-patterned setting username and password for logbook
    /// </summary>
    /// <param name="username">Logbook username</param>
    /// <param name="password">Logbook password</param>
    /// <returns>Updated HomeworkTracker</returns>
    /// <exception cref="ArgumentNullException">Username or password is empty</exception>
    public HomeworkTracker AddCredentials(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentNullException(nameof(username));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));

        _logbookDriver.Username = username;
        _logbookDriver.Password = password;

        return this;
    }

    /// <summary>
    /// Builder-patterned setting group name for homework tracking
    /// </summary>
    /// <param name="groupName">Name of group to track homeworks</param>
    /// <returns>Updated HomeworkTracker</returns>
    /// <exception cref="ArgumentNullException">Group name is empty</exception>
    public HomeworkTracker AddTargetGroupName(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            throw new ArgumentNullException(nameof(groupName));

        _logbookDriver.GroupName = groupName;

        return this;
    }

    /// <summary>
    /// Builder-patterned setting count of given homeworks
    /// </summary>
    /// <param name="totalHomeworksCount">Count of given homeworks</param>
    /// <returns>Updated HomeworkTracker</returns>
    /// <exception cref="ArgumentOutOfRangeException">Total homeworks count less than 1</exception>
    public HomeworkTracker AddTotalHomeworksCount(int totalHomeworksCount)
    {
        if (totalHomeworksCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalHomeworksCount));

        _logbookDriver.TotalHomeworksCount = totalHomeworksCount;

        return this;
    }

    /// <summary>
    /// Builder-patterned setting required homeworks percent
    /// </summary>
    /// <param name="requiredHomeworksPercent">Percent of required completed homeworks</param>
    /// <returns>Updated HomeworkTracker</returns>
    /// <exception cref="ArgumentOutOfRangeException">Required percent less than 0 or more than 100</exception>
    public HomeworkTracker AddRequiredHomeworksPercent(int requiredHomeworksPercent)
    {
        if (requiredHomeworksPercent < 0 || requiredHomeworksPercent > 100)
            throw new ArgumentOutOfRangeException(nameof(requiredHomeworksPercent));

        _logbookDriver.RequiredHomeworksPercent = requiredHomeworksPercent;

        return this;
    }

    /// <summary>
    /// Delegate for error event
    /// </summary>
    /// <param name="sender">HomeworkTracker than invoked event</param>
    /// <param name="e">Event args with message etc</param>
    public delegate void HomeworkTrackerDelegate(object sender, HomeworkTrackerEventArgs e);
}
