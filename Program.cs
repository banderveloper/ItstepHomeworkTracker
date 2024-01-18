using ItstepHomeworkTracker;


const string LOGBOOK_LOGIN = "Kalnicki_Nikita";
const string LOGBOOK_PASSWORD = "BpZ4bjS5871X";
const string GROUP_NAME = "Н4311";
const int TOTAL_HOMEWORKS_COUNT = 11;

var logbook = new LogbookWebDriver(LOGBOOK_LOGIN, LOGBOOK_PASSWORD, GROUP_NAME, TOTAL_HOMEWORKS_COUNT);

logbook.Start();

// Console.ReadLine();

