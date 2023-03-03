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
using DemoTest.WpfView.Helpers;
using ViewModels;

namespace DemoTest.WpfView.Pages
{
    /// <summary>
    /// Interaction logic for CaptchaPage.xaml
    /// </summary>
    public partial class CaptchaPage : Page
    {
        public CaptchaPage()
        {
            InitializeComponent();
            this.Loaded += (_, _) => TxtBox.Focus();
            if (DataContext is CaptchaViewModel viewModel)
            {
                viewModel.Ok = () => Navigate(NavigateTo.Login, this);
                viewModel.Focus = () => TxtBox.Focus();
            }
        }
    }
}
