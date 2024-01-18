using ItstepHomeworkTracker;


const string LOGBOOK_LOGIN = "Kalnicki_Nikita";
const string LOGBOOK_PASSWORD = "BpZ4bjS5871X";
const string GROUP_NAME = "3211";
const int TOTAL_HOMEWORKS_COUNT = 12;

var logbook = new LogbookWebDriver(LOGBOOK_LOGIN, LOGBOOK_PASSWORD, GROUP_NAME, TOTAL_HOMEWORKS_COUNT);

logbook.Start();

// Console.ReadLine();

