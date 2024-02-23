using System.IO;
using System.Net;

namespace ItstepHomeworkTracker.Desktop;

/// <summary>
/// Required files and folders existance ensurer (or downloader if files/folders does not exists)
/// </summary>
public static class FileEnsurer
{
    /// <summary>
    /// Direct link to download template.html file
    /// </summary>
    private static string _templateFileDownloadLink =
        "https://drive.google.com/uc?id=1dTHKe_C9Cpwahx1TI2WDSgIe37dZyaky&export=download";

    /// <summary>
    /// Direct link to download selenium-manager.exe
    /// </summary>
    private static string _seleniumManagerDownloadLink =
        "https://drive.google.com/uc?id=1F9ThDj1h6V3F2SZfAYw2u9gFrhzSQv3c&export=download";

    /// <summary>
    /// Ensure that all directories and files are exists, if not - create or download
    /// </summary>
    public static void Ensure()
    {
        Directory.CreateDirectory("results");
        Directory.CreateDirectory("selenium-manager/windows");

        if (!File.Exists("template.html"))
            DownloadTemplateFile();
        
        if(!File.Exists("selenium-manager/windows/selenium-manager.exe"))
            DownloadSeleniumManagerFile();
    }

    /// <summary>
    /// Download template.html file from mediafire hosting
    /// </summary>
    private static void DownloadTemplateFile()
    {
        using var client = new WebClient();
        client.DownloadFile(_templateFileDownloadLink, "template.html");
    }

    /// <summary>
    /// Download selenium-manager.exe from mediafire hosting
    /// </summary>
    private static void DownloadSeleniumManagerFile()
    {
        using var client = new WebClient();
        client.DownloadFile(_seleniumManagerDownloadLink, "selenium-manager/windows/selenium-manager.exe");
    }
}