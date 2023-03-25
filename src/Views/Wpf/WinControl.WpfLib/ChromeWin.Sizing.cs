using System;
using System.Windows.Input;
using System.Windows;
using WinControl.WpfLib.NativeTools;
using Point = System.Windows.Point;

// ReSharper disable InconsistentNaming
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace WinControl.WpfLib
{
    public partial class ChromeWin
    {
        protected virtual Thickness GetDefaultMarginForDpi() => Helper.GetCurrentDpi() switch
        {
            120 => new Thickness(7, 7, 4, 5),
            144 => new Thickness(7, 7, 3, 1),
            168 => new Thickness(6, 6, 2, 0),
            192 or 240 => new Thickness(6, 6, 0, 0),
            _ => new Thickness(8, 8, 8, 8)
        };

        protected virtual Thickness GetFromMinimizedMarginForDpi() => Helper.GetCurrentDpi() switch
        {
            120 => new Thickness(6, 6, 4, 6),
            144 => new Thickness(7, 7, 4, 4),
            168 or 192 => new Thickness(6, 6, 2, 2),
            240 => new Thickness(6, 6, 0, 0),
            _ => new Thickness(7, 7, 5, 7)
        };
        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            var result = WorkingArea;
            _previousScreenBounds = new Point(result.Width, result.Height);
        }

        private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
        {
            var result = WorkingArea;
            _previousScreenBounds = new Point(result.Width, result.Height);
            RefreshWindowState();
        }

        private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                HeightBeforeMaximize = ActualHeight;
                WidthBeforeMaximize = ActualWidth;
                return;
            }
            if (WindowState == WindowState.Maximized)
            {
                var result = WorkingArea;
                if (_previousScreenBounds.X != result.Width || _previousScreenBounds.Y != result.Height)
                {
                    _previousScreenBounds = new Point(result.Width, result.Height);
                    RefreshWindowState();
                }
            }
        }

        private void OnStateChanged(object? sender, EventArgs e)
        {
            var result = WorkingArea;
            Thickness thickness = new (0);
            if (WindowState != WindowState.Maximized)
            {
                var currentDPIScaleFactor = Helper.GetCurrentDpiScaleFactor();
                MaxHeight = (result.Height + 16) / currentDPIScaleFactor;
                MaxWidth = double.PositiveInfinity;

                if (WindowState != WindowState.Maximized)
                    SetMaximizeButtonsVisibility(Visibility.Visible, Visibility.Collapsed);
            }
            else
            {

                thickness = GetDefaultMarginForDpi();
                if (PreviousState == WindowState.Minimized || Left == _positionBeforeDrag.X && Top == _positionBeforeDrag.Y)
                {
                    thickness = GetFromMinimizedMarginForDpi();
                }

                SetMaximizeButtonsVisibility(Visibility.Collapsed, Visibility.Visible);
            }

            if (LayoutRoot != null) LayoutRoot.Margin = thickness;
            PreviousState = WindowState;
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (!_isMouseButtonDown) return;

            var currentDPIScaleFactor = Helper.GetCurrentDpiScaleFactor();
            var position = e.GetPosition(this);
            System.Diagnostics.Debug.WriteLine(position);
            var screen = PointToScreen(position);
            var x = _mouseDownPosition.X - position.X;
            var y = _mouseDownPosition.Y - position.Y;
            if (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) > 1)
            {
                var actualWidth = _mouseDownPosition.X;

                if (_mouseDownPosition.X <= 0) actualWidth = 0;
                else if (_mouseDownPosition.X >= ActualWidth) actualWidth = WidthBeforeMaximize;

                if (WindowState == WindowState.Maximized)
                {
                    ToggleWindowState();
                    Top = (screen.Y - position.Y) / currentDPIScaleFactor;
                    Left = (screen.X - actualWidth) / currentDPIScaleFactor;
                    CaptureMouse();
                }

                _isManualDrag = true;

                Top = (screen.Y - _mouseDownPosition.Y) / currentDPIScaleFactor;
                Left = (screen.X - actualWidth) / currentDPIScaleFactor;
            }
        }


        private void OnMouseButtonUp(object? sender, MouseButtonEventArgs e)
        {
            _isMouseButtonDown = false;
            _isManualDrag = false;
            ReleaseMouseCapture();
        }

        private void RefreshWindowState()
        {
            if (WindowState == WindowState.Maximized)
            {
                ToggleWindowState();
                ToggleWindowState();
            }
        }
    }
}
