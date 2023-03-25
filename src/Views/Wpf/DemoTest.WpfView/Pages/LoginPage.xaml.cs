using System.Windows;
using System.Windows.Controls;
using DemoTest.WpfView.Helpers;

namespace DemoTest.WpfView.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Navigate(NavigateTo.Captcha, this);
        }
    }
}
