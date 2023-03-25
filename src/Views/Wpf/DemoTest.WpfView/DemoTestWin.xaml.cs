using DemoTest.WpfView.Helpers;
using System.Windows;

namespace DemoTest.WpfView
{
    /// <summary>
    /// Interaction logic for DemoTestWin.xaml
    /// </summary>
    public partial class DemoTestWin 
    {
        public DemoTestWin()
        {
            InitializeComponent();
            Navigate(NavigateTo.Captcha, this);
        }
    }
}
