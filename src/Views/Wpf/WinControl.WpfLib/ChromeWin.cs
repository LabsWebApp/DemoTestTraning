using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using FontAwesome6.Fonts;
using WinControl.WpfLib.NativeTools;
using Point = System.Windows.Point;
using static WinControl.WpfLib.NativeTools.Helper;
using static WinControl.WpfLib.NativeTools.Utils;
using static WinControl.WpfLib.NativeTools.Sends.Keyboard;

namespace WinControl.WpfLib;

public partial class ChromeWin : Window
{
    private HwndSource? _hwndSource;

    private bool _isMouseButtonDown;
    private bool _isManualDrag;
    private Point _mouseDownPosition;
    private Point _positionBeforeDrag;
    private Point _previousScreenBounds;

    private Rectangle WorkingArea => GetWorkingArea(new WindowInteropHelper(this).Handle);

    public Grid? WindowRoot { get; private set; }
    public Grid? LayoutRoot { get; private set; }
    public Button? MinimizeButton { get; private set; }
    public Button? MaximizeButton { get; private set; }
    public Button? RestoreButton { get; private set; }
    public Button? CloseButton { get; private set; }
    public Grid? HeaderBar { get; private set; }
    public double HeightBeforeMaximize { get; private set; }
    public double WidthBeforeMaximize { get; private set; }
    public WindowState PreviousState { get; private set; }

    static ChromeWin() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeWin),
            new FrameworkPropertyMetadata(typeof(ChromeWin)));

    public ChromeWin()
    {
        var currentDpiScaleFactor = Helper.GetCurrentDpiScaleFactor();
        SizeChanged += OnSizeChanged;
        StateChanged += new EventHandler(OnStateChanged);
        Loaded += new RoutedEventHandler(OnLoaded);
        MaxHeight = (WorkingArea.Height + 16) / currentDpiScaleFactor;
        SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
        AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnMouseButtonUp), true);
        AddHandler(MouseMoveEvent, new MouseEventHandler(OnMouseMove));

    }

    public T? GetRequiredTemplateChild<T>(string childName) where T : DependencyObject => GetTemplateChild(childName) as T;

    public override void OnApplyTemplate()
    {
        WindowRoot = GetRequiredTemplateChild<Grid>("WindowRoot");
        LayoutRoot = GetRequiredTemplateChild<Grid>("LayoutRoot");
        MinimizeButton = GetRequiredTemplateChild<Button>("MinimizeButton");
        MaximizeButton = GetRequiredTemplateChild<Button>("MaximizeButton");
        RestoreButton = GetRequiredTemplateChild<Button>("RestoreButton");
        CloseButton = GetRequiredTemplateChild<Button>("CloseButton");
        HeaderBar = GetRequiredTemplateChild<Grid>("PART_HeaderBar");

        if (LayoutRoot != null && WindowState == WindowState.Maximized) LayoutRoot.Margin = GetDefaultMarginForDpi();

        if (CloseButton != null) CloseButton.Click += CloseButton_Click;

        if (MinimizeButton != null) MinimizeButton.Click += MinimizeButton_Click;

        if (RestoreButton != null)
        {
            RestoreButton.Click += RestoreButton_Click;
            if (RestoreButton.Content is FontAwesome font)
            {
                //font.MouseEnter -= OnMinRestoreButtonMouseEnterLeave;
                //font.MouseLeave -= OnMinRestoreButtonMouseEnterLeave;
                font.MouseEnter += OnMinRestoreButtonMouseEnterLeave;
                //font.MouseLeave += OnMinRestoreButtonMouseEnterLeave;
            }
        }

        if (MaximizeButton != null)
        {
            MaximizeButton.Click += MaximizeButton_Click;
            if (MaximizeButton.Content is FontAwesome font)
            {
                //font.MouseEnter -= OnMinRestoreButtonMouseEnterLeave;
                //font.MouseLeave -= OnMinRestoreButtonMouseEnterLeave;
                font.MouseEnter += OnMinRestoreButtonMouseEnterLeave;
                //font.MouseLeave += OnMinRestoreButtonMouseEnterLeave;
            }
        }

        HeaderBar?.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnHeaderBarMouseLeftButtonDown));

        base.OnApplyTemplate();
    }


    protected override void OnInitialized(EventArgs e)
    {
        SourceInitialized += OnSourceInitialized;
        base.OnInitialized(e);
    }

    protected virtual void OnHeaderBarMouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
    {
        if (_isManualDrag) return;

        var position = e.GetPosition(this);
        var headerBarHeight = 36;
        var leftmostClickableOffset = 50;

        if (position.X - LayoutRoot!.Margin.Left <= leftmostClickableOffset && position.Y <= headerBarHeight)
        {
            if (e.ClickCount != 2) OpenSystemContextMenu(e);
            else Close();
            e.Handled = true;
            return;
        }

        if (e.ClickCount == 2 && ResizeMode == ResizeMode.CanResize)
        {
            ToggleWindowState();
            return;
        }

        if (WindowState == WindowState.Maximized)
        {
            _isMouseButtonDown = true;
            _mouseDownPosition = position;
        }
        else
        {
            try
            {
                _positionBeforeDrag = new Point(Left, Top);
                DragMove();
            }
            catch
            {
                // ignored
            }
        }
    }

    protected void ToggleWindowState() => 
        WindowState = WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
    int count = 0;
    protected virtual void OnMinRestoreButtonMouseEnterLeave(object? sender, MouseEventArgs e)
    {
        Press(Key.LWin);
        Type(Key.Z);
        Release(Key.LWin);
        count++;
    }

    private void MaximizeButton_Click(object? sender, RoutedEventArgs e) => ToggleWindowState();

    private void RestoreButton_Click(object? sender, RoutedEventArgs e) => ToggleWindowState();

    private void MinimizeButton_Click(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseButton_Click(object? sender, RoutedEventArgs e) => Close();

    private void OnSourceInitialized(object? sender, EventArgs e) => _hwndSource = (HwndSource)PresentationSource.FromVisual(this);

    private void SetMaximizeButtonsVisibility(Visibility maximizeButtonVisibility, Visibility reverseMaximizeButtonVisibility)
    {
        if (MaximizeButton != null) MaximizeButton.Visibility = maximizeButtonVisibility;
        if (RestoreButton != null) RestoreButton.Visibility = reverseMaximizeButtonVisibility;
    }

    private void OpenSystemContextMenu(MouseEventArgs e)
    {
        var position = e.GetPosition(this);
        var screen = PointToScreen(position);
        var num = 36;
        if (!(position.Y < num)) return;

        var handle = (new WindowInteropHelper(this)).Handle;
        var systemMenu = GetSystemMenu(handle, false);
        EnableMenuItem(systemMenu, 61488, WindowState != WindowState.Maximized ? 0 : (uint)1);
        var num1 = TrackPopupMenuEx(
            systemMenu, 
            TPM_LEFTALIGN | TPM_RETURNCMD, 
            Convert.ToInt32(screen.X + 2), 
            Convert.ToInt32(screen.Y + 2), 
            handle, nint.Zero);

        if (num1 == 0) return;

        PostMessage(handle, 274, new nint(num1), nint.Zero);
    }
}