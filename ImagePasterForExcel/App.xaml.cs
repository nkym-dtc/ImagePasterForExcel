using ImagePasterForExcel.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ImagePasterForExcel
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);

        private readonly System.Threading.Mutex _mutex = new System.Threading.Mutex(false, Application.ResourceAssembly.GetName().Name);


        public App()
        {

            AttachConsole(-1);

            if (_mutex.WaitOne(0, false)) return;

            MessageBox.Show("既に起動しています。", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            _mutex.Close();
            Current.Shutdown();

        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            System.Console.WriteLine("Loading application... ");

            foreach (var arg in e.Args)
            {
                if (arg == "dev") MainWindowViewModel.IsDevMode = true;
            }

            var mainWindow = new MainWindow();
            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.Show();
            });

            System.Console.WriteLine("Start");
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            System.Console.WriteLine("Quit");
            try
            {
                _mutex.ReleaseMutex();
            }
            catch
            {
                // ignored
            }

            try
            {
                _mutex.Close();
            }
            catch
            {
                // ignored
            }
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            SystemLogger.Error(e.Exception);

            MessageBox.Show("エラーが発生しました。\nプログラムを終了します。", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            try
            {
                _mutex.ReleaseMutex();
            }
            catch
            {
                // ignored
            }

            try
            {
                _mutex.Close();
            }
            catch
            {
                // ignored
            }

            Application.Current.Shutdown();
        }




    }
}
