using ItstepHomeworkTracker;

if (args.Length < 2)
{
    Console.WriteLine("Launch error. Group name and total homeworks count must be given in args");
    return;
}

const string logbookLogin = "Kalnicki_Nikita";
const string logbookPassword = "BpZ4bjS5871X";
var groupName = args[0];
var totalHomeworksCount = int.Parse(args[1]);

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

