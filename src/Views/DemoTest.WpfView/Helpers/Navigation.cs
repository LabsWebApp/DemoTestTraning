using DemoTest.WpfView.Pages;
using System;
using System.Windows.Controls;

namespace DemoTest.WpfView.Helpers;

internal static class Navigation
{
    internal static bool Navigate(NavigateTo target, DemoTestWin win) =>
        win.MainFrame.Navigate(GetPage(target));

    internal static bool Navigate(NavigateTo target, Page page) =>
        page.NavigationService!.Navigate(GetPage(target));

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

