using System.Windows;

namespace DemoTest.WpfView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new DemoTestWin().Show();
        }
    }
}
