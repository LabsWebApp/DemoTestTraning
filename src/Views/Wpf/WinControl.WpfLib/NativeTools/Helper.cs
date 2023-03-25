using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows;
using static WinControl.WpfLib.NativeTools.Utils;
using Point = System.Windows.Point;
using System.Windows.Interop;

namespace WinControl.WpfLib.NativeTools;

internal static class Helper
{
    public static int GetCurrentDpi() =>
        (int)typeof(SystemParameters)
            .GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static)
            !.GetValue(null, null)!;

    public static double GetCurrentDpiScaleFactor() => (double)GetCurrentDpi() / 96;

    public static Point GetMouseScreenPosition()
    {
        var w32Mouse = new Win32Point();
        GetCursorPos(ref w32Mouse);

        return new Point(w32Mouse.X, w32Mouse.Y);
    }

    public static Rectangle GetWorkingArea(nint handle)
    {
        var hMonitor = MonitorFromWindow(handle, MONITOR_DEFAULTTONEAREST);
        var lpmi = new MonitorInfo { Size = 40 };
        if (!GetMonitorInfo(hMonitor, ref lpmi)) throw new Exception("No monitor");
        var result = lpmi.WorkArea.ToInt32Rect();
        return new Rectangle(result.X, result.Y, result.Width, result.Height);
    }
}