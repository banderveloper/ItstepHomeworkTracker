using ItstepHomeworkTracker;

Console.Write("Input logbook login: ");
string logbookLogin = Console.ReadLine();

Console.Write("Input logbook password: ");
string logbookPassword = Console.ReadLine();

Console.Write("Input group name: ");
string groupName = Console.ReadLine();

Console.Write("Input total count of given homeworks: ");
int totalHomeworksCount = int.Parse(Console.ReadLine());

Console.WriteLine("Input requred percent of homeworks: ");
int requiredHomeworksPercent = int.Parse(Console.ReadLine());

var logbook = new LogbookWebDriver
{
    AccountUsername = logbookLogin,
    AccountPassword = logbookPassword,
    TotalHomeworksCount = totalHomeworksCount,
    TargetGroupName = groupName,
    RequiredHomeworksPercent = requiredHomeworksPercent
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

