using ItstepHomeworkTracker;

const string logbookLogin = "Kalnicki_Nikita";
const string logbookPassword = "BpZ4bjS5871X";
const string groupName = "ПВ212";
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
logbook.OnErrorMessageSent += (sender, e) =>
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(e.LastLogMessage);
    Console.ResetColor();
};
logbook.OnFinished += (sender, e) =>
{
    Console.WriteLine(e.LastLogMessage);
    Environment.Exit(0);
};

logbook.Start();

Console.ReadLine();

