using ControlzEx.Theming;
using ImagePasterForExcel.Model;
using ImagePasterForExcel.Properties;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using MaterialDialog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImagePasterForExcel
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private SplashWindow _sw;

        MainWindowViewModel _mwvm;

        public MainWindow()
        {
          
            _mwvm = FindResource("mwvm") as MainWindowViewModel;

            ModifyTheme();

            InitializeComponent();
        }

        private async void MainMetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                /* アセンブリバージョンが変わるときにユーザ設定を引き継ぐ */
                var assemblyVersion = _mwvm.Version;
                if (Settings.Default.AssemblyVersion != assemblyVersion)
                {
                    Settings.Default.Upgrade();
                    Settings.Default.AssemblyVersion = assemblyVersion;
                    Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                bool? result = await MaterialDialogUtil.ShowMaterialMessageYesNoDialog(this, "Warning", "設定ファイルが壊れています。\n初期化or前バージョンの設定で再起動しますか？");
                var filePath = ((ConfigurationErrorsException)ex.InnerException).Filename;
                File.Delete(filePath);
                if (result.HasValue && result.Value == true)
                {
                    var appName = Application.ResourceAssembly.GetName().Name;
                    Process.Start(appName + ".exe");
                    Close();
                    return;
                }
                Close();
                return;
            }


            //if (GetWindow(this) is Window window) window.KeyDown += _mwvm.HandleKeyPress;

        }



        private void ModifyTheme()
        {
            ThemeSelector.IsDarkTheme = Settings.Default.IsDarkTheme;
            ThemeSelector.PrimaryColorCode = Settings.Default.PrimaryColorCode;
            ThemeSelector.PrimaryColor = Settings.Default.PrimaryColor;
            ThemeSelector.AccentColorCode = Settings.Default.AccentColorCode;

            _mwvm.IsDarkTheme = ThemeSelector.IsDarkTheme;


            try
            {
                ThemeSelector.ModifyTheme(theme => theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString(ThemeSelector.PrimaryColorCode)));
            }
            catch
            {
                // ignored
            }

            try
            {
                ThemeSelector.ModifyTheme(theme => theme.SetSecondaryColor((Color)ColorConverter.ConvertFromString(ThemeSelector.AccentColorCode)));
            }
            catch
            {
                // ignored
            }

            try
            {

                switch (ThemeSelector.PrimaryColor)
                {
                    case "lightblue":
                        ThemeSelector.PrimaryColor = "Blue";
                        break;
                    case "deeppurple":
                        ThemeSelector.PrimaryColor = "Purple";
                        break;
                    case "purple":
                        ThemeSelector.PrimaryColor = "Violet";
                        break;

                    case "lightgreen":
                        ThemeSelector.PrimaryColor = "Green";
                        break;
                    case "deeporange":
                        ThemeSelector.PrimaryColor = "Orange";
                        break;
                    case "bluegrey":
                        ThemeSelector.PrimaryColor = "Steel";
                        break;
                    case "grey":
                        ThemeSelector.PrimaryColor = "Steel";
                        break;

                };

                var mode = ThemeSelector.IsDarkTheme ? "Dark" : "Light";
                ThemeManager.Current.ChangeTheme(Application.Current, $"{mode}.{ThemeSelector.PrimaryColor}");
            }
            catch
            {
                // ignored
            }
        }

        private void MainMetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.PrimaryColorCode = ThemeSelector.PrimaryColorCode;
            Settings.Default.PrimaryColor = ThemeSelector.PrimaryColor;
            Settings.Default.AccentColorCode = ThemeSelector.AccentColorCode;
            Settings.Default.IsDarkTheme = ThemeSelector.IsDarkTheme;
            Settings.Default.Save();
        }
    }
}
