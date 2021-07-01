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

namespace MaterialDialog
{
    /// <summary>
    /// MessageDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageDialog : UserControl
    {
        public MessageDialog(string title, string message)
        {
            InitializeComponent();
            title_tbk.Text = title;
            message_tbk.Text = message;
        }

        public MessageDialog(string title, string message, bool yesno_btns = false) : this(title, message)
        {
            if (yesno_btns)
            {
                default_btns_sp.Visibility = Visibility.Collapsed;
            }
            else
            {
                yesno_btns_sp.Visibility = Visibility.Collapsed;
            }
        }
    }
}
