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
using System.Windows.Shapes;

namespace WpfViewTest.Windows
{
    /// <summary>
    /// Логика взаимодействия для FontAwesomeWin.xaml
    /// </summary>
    public partial class FontAwesomeWin 
    {
        public FontAwesomeWin()
        {
            InitializeComponent();
            Btn.Content = "";
        }

        private void ChromeWin_MouseMove(object sender, MouseEventArgs e)
        {
            var element = e.MouseDevice.DirectlyOver as FrameworkElement;
            Title = element?.Name ?? "no name";
        }
    }
}
