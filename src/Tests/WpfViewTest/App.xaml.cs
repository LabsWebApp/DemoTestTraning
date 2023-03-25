using System.Windows;
using WpfViewTest.Windows;

namespace WpfViewTest;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App 
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Window win = new FontAwesomeWin();
        win.Show();
    }
}