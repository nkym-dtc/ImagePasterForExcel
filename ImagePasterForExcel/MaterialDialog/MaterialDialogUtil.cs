using ImagePasterForExcel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MaterialDialog
{
    public class MaterialProgressDialogController
    {
        public DialogHost dh = null;
        private ProgressDialog pd;
        public MaterialProgressDialogController(ProgressDialog pd)
        {
            this.pd = pd;
        }

        public void SetMessage(string message)
        {
            pd.message_tbk.Text = message;
        }

        public void Close()
        {
            if (dh != null)
            {
                dh.DialogContent = null;
                dh.IsOpen = false;
            }
        }

        public async Task CloseWait(int millisecond = 500)
        {
            Close();
            await Task.Delay(millisecond);
        }
    }

    public class MaterialDialogUtil
    {
        public static Task<MaterialProgressDialogController> ShowMaterialProgressDialog(Window window, string message)
        {

            var pd = new ProgressDialog(message)
            {
                MaxWidth = window.Width / 2
            };
            var dh = GetFirstDialogHost(window);
            dh.DialogContent = pd;
            dh.IsOpen = true;
            var mpdc = new MaterialProgressDialogController(pd) { dh = dh };

            return Task.FromResult(mpdc);
        }

        public static async Task ShowProgress(string message, Func<Task> callback)
        {
            if (!(Application.Current.MainWindow is MainWindow currentWindow)) return;

            await currentWindow.Dispatcher.Invoke(async () =>
            {
                var con = await ShowMaterialProgressDialog(currentWindow, message);
                try
                {
                    await callback();
                }
                catch
                {
                    con.Close();
                    throw;
                }

                con.Close();
            });

        }

        private static DialogHost GetFirstDialogHost(Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));

            var dialogHost = VisualDepthFirstTraversal(window).OfType<DialogHost>().FirstOrDefault();

            if (dialogHost == null)
                throw new InvalidOperationException("Unable to find a DialogHost in visual tree");

            return dialogHost;
        }

        public static IEnumerable<DependencyObject> VisualDepthFirstTraversal(DependencyObject node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            yield return node;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
            {
                var child = VisualTreeHelper.GetChild(node, i);
                foreach (var descendant in VisualDepthFirstTraversal(child))
                {
                    yield return descendant;
                }
            }
        }

        public static async Task<bool?> ShowMaterialMessageDialog(Window window, string title, string message)
        {
            bool? re = null;
            MessageDialog md = new MessageDialog(title, message, false);
            md.MaxWidth = window.ActualWidth / 2;
            var result = await DialogHostEx.ShowDialog(window, md);
            re = result as bool?;
            return re;
        }



        public static async Task<bool?> ShowMaterialMessageYesNoDialog(Window window, string title, string message)
        {
            bool? re = null;
            MessageDialog md = new MessageDialog(title, message, true);
            md.MaxWidth = window.ActualWidth / 2;
            var result = await DialogHostEx.ShowDialog(window, md);
            re = result as bool?;
            return re;
        }


        public static async Task<object> ShowDialog(object dialog)
        {
            object ret = null;

            if (!(Application.Current.MainWindow is MainWindow mainWindow)) return ret;

            await mainWindow.Dispatcher.Invoke(async () =>
            {
                ret = await mainWindow.ShowDialog(dialog);
            });

            return ret;
        }

        public static async Task<bool?> ShowMessage(string title, string message)
        {
            if (!(Application.Current.MainWindow is MainWindow mainWindow)) return null;

            bool? re = null;
            var md = new MessageDialog(title, message, false)
            {
                MaxWidth = mainWindow.ActualWidth / 2
            };

            await mainWindow.Dispatcher.Invoke(async () =>
            {
                re = await mainWindow.ShowDialog(md) as bool?;

            });

            return re;
        }

        public static async Task<bool?> ShowYN(string title, string message)
        {
            if (!(Application.Current.MainWindow is MainWindow mainWindow)) return null;
            bool? re = null;
            var md = new MessageDialog(title, message, true)
            {
                MaxWidth = mainWindow.ActualWidth / 2
            };

            await mainWindow.Dispatcher.Invoke(async () =>
            {
                re = await mainWindow.ShowDialog(md) as bool?;

            });

            return re;
        }
    }

    public class MaterialProgressTaskDialog
    {
        private readonly Window window;
        private readonly ProgressDialog progressDialog;
        public MaterialProgressTaskDialog(Window window)
        {
            this.window = window;
            progressDialog = new ProgressDialog();
        }

        public async Task Show(string message, Func<MaterialProgressTaskDialog, Task> execute, bool indicator = true)
        {
            SetMessage(message);
            SetProgressValue(0);
            SetIndicator(indicator);
            await DialogHostEx.ShowDialog(window, progressDialog, async delegate (object sender, DialogOpenedEventArgs args)
            {
                await execute(this);
                args.Session.Close();
            });
        }

        public void SetMessage(string message)
        {
            progressDialog.message_tbk.Text = message;
        }

        public void SetProgressValue(int value)
        {
            progressDialog.progress2.Value = value;
        }

        public void SetIndicator(bool isIndicator)
        {
            if (isIndicator)
            {
                progressDialog.progress1.Visibility = Visibility.Visible;
                progressDialog.progress2.Visibility = Visibility.Hidden;
            }
            else
            {
                progressDialog.progress1.Visibility = Visibility.Hidden;
                progressDialog.progress2.Visibility = Visibility.Visible;
            }
        }
    }
}
