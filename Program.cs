using ItstepHomeworkTracker;

const string LOGBOOK_LOGIN = "Kalnicki_Nikita";
const string LOGBOOK_PASSWORD = "BpZ4bjS5871X";
const string GROUP_NAME = "П21";
const int TOTAL_HOMEWORKS_COUNT = 15;

var logbook = new LogbookWebDriver
{
    AccountUsername = LOGBOOK_LOGIN,
    AccountPassword = LOGBOOK_PASSWORD,
    TotalHomeworksCount = TOTAL_HOMEWORKS_COUNT,
    TargetGroupName = GROUP_NAME,
    RequiredHomeworksPercent = 80
};

logbook.Start();

Console.ReadLine();

