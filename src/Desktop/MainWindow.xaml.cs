using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ItstepHomeworkTracker.Desktop.Extensions;
using ItstepHomeworkTracker.Library;

namespace ItstepHomeworkTracker.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private HomeworkTracker _tracker;
    private Brush _defaultTextBoxBrush;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnLoad(object sender, RoutedEventArgs e)
    {
        _defaultTextBoxBrush = UsernameTextBox.BorderBrush;
        RequiredHomeworksPercentTextBox.Text = "80";

        if (!IsLogbookAvailable())
        {
            MessageBox.Show(
                "Logbook is not available. Check your internet connection and VPN enabled (if you are outside the academy)",
                "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void OnExitButtonClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OnStartButtonClick(object sender, RoutedEventArgs e)
    {
        if (!IsTextBoxesFilled())
        {
            HighlightEmptyTextboxes();
            return;
        }

        _tracker = new HomeworkTracker()
            .AddCredentials(UsernameTextBox.Text, PasswordTextBox.Password)
            .AddTargetGroupName(GroupNameTextBox.Text)
            .AddTotalHomeworksCount(int.Parse(HomeworksCountTextBox.Text))
            .AddRequiredHomeworksPercent(int.Parse(RequiredHomeworksPercentTextBox.Text));

        _tracker.OnParsingSucceed += OnTrackerFinish;
        _tracker.OnParsingError += OnTrackerError;

        _tracker.Start();
    }

    private void OnNumberTextBoxValidate(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void OnTextBoxTextChange(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        textBox.BorderBrush = textBox.Text.Length > 0 ? _defaultTextBoxBrush : Brushes.IndianRed;
    }

    private void OnPasswordBoxChange(object sender, RoutedEventArgs e)
    {
        var passwordBox = sender as PasswordBox;
        passwordBox.BorderBrush = passwordBox.Password.Length > 0 ? _defaultTextBoxBrush : Brushes.IndianRed;
    }

    private void OnTrackerError(object sender, HomeworkTrackerEventArgs e)
    {
        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void OnTrackerFinish(object sender, HomeworkTrackerEventArgs e)
    {
        // 3322.html
        var resultFileShortName = GroupNameTextBox.Text + ".html";
        // C:\Users....
        var resultFileFullName = new FileInfo($"./results/{resultFileShortName}").FullName;

        var messageBoxResult = MessageBox.Show($"File {resultFileShortName} is created. Do you wanna open it now?",
            "Done!",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (messageBoxResult == MessageBoxResult.Yes)
        {
            var processStartInfo = new ProcessStartInfo(resultFileFullName) { UseShellExecute = true };
            Process.Start(processStartInfo);
        }
    }

    private bool IsTextBoxesFilled()
    {
        return
            UsernameTextBox.Text.Length * PasswordTextBox.Password.Length *
            GroupNameTextBox.Text.Length * HomeworksCountTextBox.Text.Length *
            RequiredHomeworksPercentTextBox.Text.Length > 0;
    }

    private void HighlightEmptyTextboxes()
    {
        var textBoxes = this.FindVisualChildren<TextBox>().Where(tb => tb.Text.Length == 0);
        foreach (var textBox in textBoxes) textBox.BorderBrush = Brushes.IndianRed;

        var passwordBox = this.FindVisualChildren<PasswordBox>().FirstOrDefault(pb => pb.Password.Length == 0);
        if (passwordBox != null) passwordBox.BorderBrush = Brushes.IndianRed;
    }

    /// <summary>
    /// Send small http request to logbook for ensuring server available
    /// </summary>
    /// <returns>Is logbook http server available</returns>
    private bool IsLogbookAvailable()
    {
        var request = (HttpWebRequest)HttpWebRequest.Create("https://logbook.itstep.org");
        request.AllowAutoRedirect = false;
        request.Method = "HEAD";
        request.Timeout = 1000;

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

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        DragMove();
    }
}