using DemoTest.WpfView.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModels;

namespace DemoTest.WpfView.Controls
{
    /// <summary>
    /// Interaction logic for CaptchaControl.xaml
    /// </summary>
    public partial class CaptchaControl : UserControl
    {
        public CaptchaControl()
        {
            InitializeComponent();

            RegToggle toggle = new(
                @"Software\Microsoft\Input\Settings",
                "EnableHwkbTextPrediction", "MultilingualEnabled");

            TxtBox.GotFocus += (_, _) => toggle.Switch(RegToggle.Toggle.Off);
            TxtBox.LostFocus += (_, _) => toggle.Switch(RegToggle.Toggle.On);
        }

        private void Button_Click(object sender, RoutedEventArgs e) => TxtBox.Focus();
    }
}
