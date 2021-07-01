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

namespace ImagePasterForExcel.Views.Panels
{
    /// <summary>
    /// DebugConsole.xaml の相互作用ロジック
    /// </summary>
    public partial class DebugConsole : Window
    {
        public DebugConsole()
        {
            InitializeComponent();
            Cmd.Focus();
        }

        private async void Cmd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
            {
               
            }
            else if (e.Key is Key.Escape)
            {
                Close();
            }
        }

        private void DebugConsole_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Cmd.Focus();
        }
    }
}
