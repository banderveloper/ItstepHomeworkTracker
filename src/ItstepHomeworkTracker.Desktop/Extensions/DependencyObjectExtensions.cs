using System.Windows;
using System.Windows.Media;

namespace ItstepHomeworkTracker.Desktop.Extensions;

public static class DependencyObjectExtensions
{
    public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject? dependencyObject) where T : DependencyObject
    {
        if (dependencyObject is null)
        {
            yield return (T)Enumerable.Empty<T>();
            yield break;
        }
        
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
        {
            var ithChild = VisualTreeHelper.GetChild(dependencyObject, i);
            if (ithChild is T t) yield return t;
            foreach (var childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
        }
    }
}