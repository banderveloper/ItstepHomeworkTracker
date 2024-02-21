using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ItstepHomeworkTracker.Library;

namespace ItstepHomeworkTracker.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private HomeworkTracker _tracker;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnExitButtonClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OnStartButtonClick(object sender, RoutedEventArgs e)
    {
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

    private void OnTrackerError(object sender, HomeworkTrackerEventArgs e)
    {
        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void OnTrackerFinish(object sender, HomeworkTrackerEventArgs e)
    {
        MessageBox.Show("Done!");
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        DragMove();
    }
}