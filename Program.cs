using ItstepHomeworkTracker;
using ItstepHomeworkTracker.BySelectors;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

const string LOGBOOK_LOGIN = "Kalnicki_Nikita";
const string LOGBOOK_PASSWORD = "BpZ4bjS5871X";
const string GROUP_NAME = "3312";
const int TOTAL_HOMEWORKS_COUNT = 11;

var logbook = new LogbookWebDriver(LOGBOOK_LOGIN, LOGBOOK_PASSWORD, GROUP_NAME, TOTAL_HOMEWORKS_COUNT);

logbook.Start();
