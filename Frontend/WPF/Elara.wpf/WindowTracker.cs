using System.Windows;

namespace Elara.wpf;
public static class WindowTracker
{
    public static Window? LastActivatedWindow { get; private set; }

    public static void Register(Window window)
    {
        window.Activated += (s, e) =>
        {
            LastActivatedWindow = window;
        };
    }
}

