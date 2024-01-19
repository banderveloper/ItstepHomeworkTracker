using ItstepHomeworkTracker;

if (args.Length < 5)
{
    Console.WriteLine("Launch error! Needed 5 launch arguments - username, password, group name, total homeworks count, required percent");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
    Environment.Exit(0);
}

string logbookLogin = args[0], logbookPassword = args[1], groupName = args[2];
int totalHomeworksCount = int.Parse(args[3]), requiredHomeworksPercent = int.Parse(args[4]);

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

