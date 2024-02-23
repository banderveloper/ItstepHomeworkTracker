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
        "https://download1654.mediafire.com/gl3uzmwv26sgFHg4RCvC3XBInQri-dirsfUAk9Ur58809hX2pMx5rFQA1CYTJ46VlVPxDOYK9SRkd_uBhcxxpljNtJ1RyybViknOM8DeJaU9Hhk8gkVG_BbAAufOtrE-L641Ymb4nY5SHNdHIZbt278TxWnLxoz5VrYQswbMOFQm/4hgh8n6s0n2jzdu/template.html";

    /// <summary>
    /// Direct link to download selenium-manager.exe
    /// </summary>
    private static string _seleniumManagerDownloadLink =
        "https://download1509.mediafire.com/kcp5ae8jzpigUCgVVv2khPZGQd7hqDHlo8J4Cr0VTmHyEBvn7qjX_JZYBRBGoR4X7hXcnq-NnQBbIQHRAhyKQN-khenCRs_12XNVVxb6_DG4tPrPxXmTraks3orZqHKASDy5Qd2M23e2cplYuc_lI_QN3RXe566M2EDYbS6MyF0x/zeyoxljbjsdrjzo/selenium-manager.exe";

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