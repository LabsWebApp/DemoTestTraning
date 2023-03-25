using DemoTest.WpfView.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DemoTest.WpfView.Helpers;

internal static class Navigation
{
    internal static bool Navigate(NavigateTo target, DemoTestWin win) =>
        win.MainFrame.Navigate(GetPage(target));

    internal static bool Navigate(NavigateTo target, Page page) =>
        page.NavigationService!.Navigate(GetPage(target));

    internal static bool Navigate(NavigateTo target, DependencyObject? child)
    {
        var res = child;
        while (res is not null)
        {
            switch (res)
            {
                case Page page:
                    return Navigate(target, page);
                case DemoTestWin win:
                    return Navigate(target, win);
                default:
                    res = (res as FrameworkElement)?.Parent;
                    break;
            }
        }
        return false;
    }

    private static Page GetPage(NavigateTo target) => target switch
    {
        NavigateTo.Captcha => new CaptchaPage(),
        NavigateTo.Login => new LoginPage(),
        NavigateTo.Edit => new EditPage(),
        NavigateTo.Registration => new RegistrationPage(),
        NavigateTo.AdminArea => new AdminPage(),
        NavigateTo.UserArea => new UserPage(),
        _ => throw new NotImplementedException()
    };


}

