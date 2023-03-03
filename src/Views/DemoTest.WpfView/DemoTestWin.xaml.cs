using DemoTest.WpfView.Helpers;
using DemoTest.WpfView.Pages;
using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace DemoTest.WpfView
{
    /// <summary>
    /// Interaction logic for DemoTestWin.xaml
    /// </summary>
    public partial class DemoTestWin : Window
    {
        public DemoTestWin()
        {
            InitializeComponent();
            Navigate(NavigateTo.Captcha, this);
        }
    }
}
