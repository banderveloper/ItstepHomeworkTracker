using ItstepHomeworkTracker;

const string logbookLogin = "Kalnicki_Nikita";
const string logbookPassword = "BpZ4bjS5871X";
const string groupName = "ПВ211";
const int totalHomeworksCount = 50;

var logbook = new LogbookWebDriver
{
    AccountUsername = logbookLogin,
    AccountPassword = logbookPassword,
    TotalHomeworksCount = totalHomeworksCount,
    TargetGroupName = groupName,
    RequiredHomeworksPercent = 80
};

logbook.Start();

Console.ReadLine();

