using ItstepHomeworkTracker;

const string logbookLogin = "Kalnicki_Nikita";
const string logbookPassword = "BpZ4bjS5871X";
const string groupName = "ПВ212qweq";
const int totalHomeworksCount = 100;

var logbook = new LogbookWebDriver
{
    AccountUsername = logbookLogin,
    AccountPassword = logbookPassword,
    TotalHomeworksCount = totalHomeworksCount,
    TargetGroupName = groupName,
    RequiredHomeworksPercent = 80
};

logbook.OnLogMessageSent += (sender, e) =>
{
    Console.WriteLine(e.LastLogMessage);
};
logbook.OnFinished += (sender, e) =>
{
    Console.WriteLine(e.LastLogMessage);
    Environment.Exit(0);
};

logbook.Start();

Console.ReadLine();

