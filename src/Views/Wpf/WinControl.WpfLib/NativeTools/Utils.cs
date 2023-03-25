using System.Runtime.InteropServices;
using System.Windows;
using System;

namespace WinControl.WpfLib.NativeTools;

internal static class Utils
{
    internal static uint TPM_LEFTALIGN = 0;

    internal static uint TPM_RETURNCMD = 256;

    internal const int MONITOR_DEFAULTTONULL = 0;
    internal const int MONITOR_DEFAULTTOPRIMARY = 1;
    internal const int MONITOR_DEFAULTTONEAREST = 2;

    #region For Mouse & Keys
    // Two special bitmasks we define to be able to grab
    // shift and character information out of a VKey.
    internal const int VKeyShiftMask = 0x0100;
    internal const int VKeyCharMask = 0x00FF;

    internal const int KeyeventfExtendedkey = 0x0001;
    internal const int KeyeventfKeyup = 0x0002;
    internal const int KeyeventfScancode = 0x0008;

    internal const int MouseeventfVirtualdesk = 0x4000;

    internal const int SMXvirtualscreen = 76;
    internal const int SMYvirtualscreen = 77;
    internal const int SMCxvirtualscreen = 78;
    internal const int SMCyvirtualscreen = 79;

    internal const int XButton1 = 0x0001;
    internal const int XButton2 = 0x0002;
    internal const int WheelDelta = 120;

    internal const int InputMouse = 0;
    internal const int InputKeyboard = 1;

    // Importing various Win32 APIs that we need for input
    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    internal static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern int MapVirtualKey(int nVirtKey, int nMapType);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern int SendInput(int nInputs, ref INPUT mi, int cbSize);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern short VkKeyScan(char ch);

    #endregion

    [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
    internal static extern nint PostMessage(nint hWnd, uint msg, nint wParam, nint lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
    internal static extern nint GetSystemMenu(nint hWnd, bool bRevert);

    [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
    internal static extern bool EnableMenuItem(nint hMenu, uint uIDEnableItem, uint uEnable);

    [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
    internal static extern int TrackPopupMenuEx(nint hmenu, uint fuFlags, int x, int y, nint hwnd, nint lptpm);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetCursorPos(ref Win32Point pt);

    [DllImport("user32.dll")]
    internal static extern nint MonitorFromWindow(nint hwnd, uint dwFlags);

    //[DllImport("User32.dll")]
    //static extern bool GetMonitorInfo(IntPtr hMonitor, [In, Out] MonitorInfoEx lpmi);

    [DllImport("user32.dll")]
    internal static extern bool GetMonitorInfo(nint hMonitor, ref MonitorInfo lpmi);
}

[StructLayout(LayoutKind.Sequential)]
internal struct Win32Point
{
    public int X;
    public int Y;
};

[Serializable]
internal struct RECT
{
    public int Left;

    public int Top;

    public int Right;

    public int Bottom;

    public int Height
    {
        get => Bottom - Top;
        set => Bottom = Top + value;
    }

    public Point Position => new((double)Left, (double)Top);

    public Size Size => new((double)Width, (double)Height);

    public int Width
    {
        get => Right - Left;
        set => Right = Left + value;
    }

    public RECT(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public RECT(Rect rect)
    {
        Left = (int)rect.Left;
        Top = (int)rect.Top;
        Right = (int)rect.Right;
        Bottom = (int)rect.Bottom;
    }

    public void Offset(int dx, int dy)
    {
        Left += dx;
        Right += dx;
        Top += dy;
        Bottom += dy;
    }

    public Int32Rect ToInt32Rect() => new(Left, Top, Width, Height);
}

//internal struct NCCALCSIZE_PARAMS
//{
//    internal RECT rect0;

//    internal RECT rect1;

//    internal RECT rect2;

//    internal nint lppos;
//}

/// <summary>
/// The MONITORINFOEX structure contains information about a display monitor.
/// The GetMonitorInfo function stores information into a MONITORINFOEX structure or a MONITORINFO structure.
/// The MONITORINFOEX structure is a superset of the MONITORINFO structure. The MONITORINFOEX structure adds a string member to contain a name
/// for the display monitor.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct MonitorInfo
//internal struct MonitorInfoEx // раскомментировать DeviceName + Init
{
    /// <summary>
    /// The size, in bytes, of the structure. Set this member to sizeof(MONITORINFOEX) (72) before calling the GetMonitorInfo function.
    /// Doing so lets the function determine the type of structure you are passing to it.
    /// </summary>
    public int Size;

    /// <summary>
    /// A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates.
    /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
    /// </summary>
    public RECT Monitor;

    /// <summary>
    /// A RECT structure that specifies the work area rectangle of the display monitor that can be used by applications,
    /// expressed in virtual-screen coordinates. Windows uses this rectangle to maximize an application on the monitor.
    /// The rest of the area in rcMonitor contains system windows such as the task bar and side bars.
    /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
    /// </summary>
    public RECT WorkArea;

    /// <summary>
    /// The attributes of the display monitor.
    ///
    /// This member can be the following value:
    ///   1 : MONITORINFOF_PRIMARY
    /// </summary>
    public uint Flags;

    ///// <summary>
    ///// A string that specifies the device name of the monitor being used. Most applications have no use for a display monitor name,
    ///// and so can save some bytes by using a MONITORINFO structure.
    ///// </summary>
    //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
    //public string DeviceName;

    //public void Init()
    //{
    //    this.Size = 40 + 2 * CCHDEVICENAME;
    //    this.DeviceName = string.Empty;
    //}
}

// Various Win32 data structures
[StructLayout(LayoutKind.Sequential)]
internal struct INPUT
{
    internal int type;
    internal INPUTUNION union;
};

[StructLayout(LayoutKind.Explicit)]
internal struct INPUTUNION
{
    [FieldOffset(0)]
    internal MOUSEINPUT mouseInput;
    [FieldOffset(0)]
    internal KEYBDINPUT keyboardInput;
};

[StructLayout(LayoutKind.Sequential)]
internal struct MOUSEINPUT
{
    internal int dx;
    internal int dy;
    internal int mouseData;
    internal int dwFlags;
    internal int time;
    internal IntPtr dwExtraInfo;
};

[StructLayout(LayoutKind.Sequential)]
internal struct KEYBDINPUT
{
    internal short wVk;
    internal short wScan;
    internal int dwFlags;
    internal int time;
    internal IntPtr dwExtraInfo;
};

[Flags]
internal enum SendMouseInputFlags
{
    Move = 0x0001,
    LeftDown = 0x0002,
    LeftUp = 0x0004,
    RightDown = 0x0008,
    RightUp = 0x0010,
    MiddleDown = 0x0020,
    MiddleUp = 0x0040,
    XDown = 0x0080,
    XUp = 0x0100,
    Wheel = 0x0800,
    Absolute = 0x8000,
};