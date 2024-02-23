using System.ComponentModel;
using System.Net;
using System.Windows;

namespace ItstepHomeworkTracker.Desktop.Windows;

/// <summary>
/// Interaction logic for SplashScreen.xaml
/// </summary>
public partial class SplashScreen : Window
{
    private string _logbookUrl = "https://logbook.itstep.org";
    private int _logbookRequestTimeout = 1000;

    public SplashScreen()
    {
        InitializeComponent();
    }

    private void OnContentRendered(object sender, EventArgs e)
    {
        var worker = new BackgroundWorker();
        worker.WorkerReportsProgress = true;
        worker.DoWork += DoWork;
        worker.ProgressChanged += OnProgressChanged;
        worker.RunWorkerAsync();
    }

    private void DoWork(object sender, DoWorkEventArgs e)
    {
        var worker = sender as BackgroundWorker;

        worker.ReportProgress(10);

        FileEnsurer.CreateMissingDirectories();
        worker.ReportProgress(25);

        FileEnsurer.DownloadMissingTemplateFile();
        worker.ReportProgress(50);

        FileEnsurer.DownloadMissingSeleniumManagerFile();
        worker.ReportProgress(75);

        if (!IsLogbookAvailable())
            MessageBox.Show(
                "Logbook is not available. Check your internet connection and VPN enabled (if you are outside the academy)",
                "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        worker.ReportProgress(100);
    }

    private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        // Change loading description text according to progress value
        // i guess bad solution but let it be temporary
        switch (e.ProgressPercentage)
        {
            case <= 10:
                ProgressText.Text = "Creating directories...";
                break;

            case <= 50:
                ProgressText.Text = "Getting missing files...";
                break;

            case <= 75:
                ProgressText.Text = "Pinging logbook...";
                break;
        }

        // Change progress bar value
        progressBar.Value = e.ProgressPercentage;

        // If loading is completed - close splash screen and open main window
        if (progressBar.Value == 100)
        {
            var mainWindow = new MainWindow();
            Close();
            mainWindow.ShowDialog();
        }
    }

    /// <summary>
    /// Send small http request to logbook for ensuring server available
    /// </summary>
    /// <returns>Is logbook http server available</returns>
    private bool IsLogbookAvailable()
    {
        var request = (HttpWebRequest)HttpWebRequest.Create(_logbookUrl);
        request.AllowAutoRedirect = false;
        request.Method = "HEAD";
        request.Timeout = _logbookRequestTimeout;

        try
        {
            request.GetResponse();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
