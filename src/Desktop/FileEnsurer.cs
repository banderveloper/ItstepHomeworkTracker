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
    /// Ensure that all directories exists, if not - create
    /// </summary>
    public static void CreateMissingDirectories()
    {
        Directory.CreateDirectory("results");
        Directory.CreateDirectory("selenium-manager/windows");
    }

    /// <summary>
    /// Download template.html file from hosting
    /// </summary>
    public static void DownloadMissingTemplateFile()
    {
        if (File.Exists("template.html"))
            return;

        using var client = new WebClient();
        client.DownloadFile(_templateFileDownloadLink, "template.html");

    }

    /// <summary>
    /// Download selenium-manager.exe from hosting
    /// </summary>
    public static void DownloadMissingSeleniumManagerFile()
    {
        if (File.Exists("selenium-manager/windows/selenium-manager.exe")) 
            return;

        using var client = new WebClient();
        client.DownloadFile(_seleniumManagerDownloadLink, "selenium-manager/windows/selenium-manager.exe");
    }
}