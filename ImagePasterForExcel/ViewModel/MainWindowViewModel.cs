using ImagePasterForExcel.Model;
using ImagePasterForExcel.ViewModel;
using ImagePasterForExcel.Views.Setup;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MaterialDialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MSAPI = Microsoft.WindowsAPICodePack;
using System.Reflection;
using Reactive.Bindings;


namespace ImagePasterForExcel
{
    public class MainWindowViewModel : BindableClasses
    {

        public string Title => Application.ResourceAssembly.GetName().Name;

        public string Version => GetAssemblyVersion();

        private bool _DialogIsOpen;
        public bool DialogIsOpen
        {
            get => _DialogIsOpen;
            set => this.SetProperty(ref _DialogIsOpen, value);
        }

        private int _SelectedMenuIndex = 0;
        public int SelectedMenuIndex
        {
            get => _SelectedMenuIndex;
            set => this.SetProperty(ref _SelectedMenuIndex, value);
        }

        public static SnackbarMessageQueue SnackbarMessageQueue { get; set; } = new SnackbarMessageQueue();

        public IEnumerable<Swatch> Swatches { get; set; } = new SwatchesProvider().Swatches;

        private bool _isDarkMode = true;
        public bool IsDarkTheme
        {
            get => _isDarkMode;
            set
            {
                SetProperty(ref _isDarkMode, value);
                ThemeSelector.IsDarkTheme = value;
                ThemeSelector.ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));

            }
        }

        public ICommand ApplyPrimaryColor => new DelegateCommand((o) =>
        {
            ThemeSelector.ApplyPrimary(o as Swatch);
        });


        public ICommand ApplyAccentColor => new DelegateCommand((o) =>
        {
            ThemeSelector.ApplyAccent(o as Swatch);
        });

        public ICommand SetupTheme => new DelegateCommand(async _ =>
        {
            await ShowDialog(new ThemeConfig());
        });

        public ICommand SelectImageDir => new DelegateCommand(_ =>
       {
           var dlg = new MSAPI::Dialogs.CommonOpenFileDialog();

           // フォルダ選択ダイアログ（falseにするとファイル選択ダイアログ）
           dlg.IsFolderPicker = true;
           // タイトル
           dlg.Title = "フォルダを選択してください";
           // 初期ディレクトリ
           dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

           if (dlg.ShowDialog() == MSAPI::Dialogs.CommonFileDialogResult.Ok)
           {
               ImageDirectory = new DirectoryInfo(dlg.FileName);
               ImageDirectoryName.Value = dlg.FileName;
           }

       });

        public ICommand SelectSaveDir => new DelegateCommand(_ =>
        {
            var dlg = new MSAPI::Dialogs.CommonOpenFileDialog();

            // フォルダ選択ダイアログ（falseにするとファイル選択ダイアログ）
            dlg.IsFolderPicker = true;
            // タイトル
            dlg.Title = "フォルダを選択してください";
            // 初期ディレクトリ
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            if (dlg.ShowDialog() == MSAPI::Dialogs.CommonFileDialogResult.Ok)
            {
                SaveDirectory = new DirectoryInfo(dlg.FileName);
                SaveDirectoryName.Value = dlg.FileName;
            }

        });

        public ICommand PasteImageToExcel => new DelegateCommand(async _ =>
       {
           await MaterialDialogUtil.ShowProgress("Wait...", async () =>
           {
               await Task.Run(() =>
               {
                   var savePath = $@"{SaveDirectory.FullName}\{SaveName}";
                   Excel.MakeExcelFile(ImageDirectory, savePath);
               });
           });
       });

        public static bool IsDevMode { get; internal set; }

       
        public DirectoryInfo ImageDirectory { get; set; }
        public ReactivePropertySlim<string> ImageDirectoryName { get; } = new ReactivePropertySlim<string>(default);

     
        public DirectoryInfo SaveDirectory { get; set; }
        public ReactivePropertySlim<string> SaveDirectoryName { get; } = new ReactivePropertySlim<string>(default);

        private string _SaveName;

        public string SaveName
        {
            get => _SaveName;
            set => SetProperty(ref _SaveName, value);
        }



        public Excel Excel { get; set; }

        public MainWindowViewModel()
        {
            Excel = new Excel();

        }

        public async Task<object> ShowDialog(object dialog)
        {
            if (DialogIsOpen) return null;
            var mainWindow = Application.Current.MainWindow as MainWindow;

            return await mainWindow.ShowDialog(dialog);
        }

        public string GetAssemblyVersion()
        {
            Assembly mainAssembly = Assembly.GetEntryAssembly();
            AssemblyName mainAssemName = mainAssembly.GetName();
            // バージョン名（AssemblyVersion属性）を取得
            Version appVersion = mainAssemName.Version;
            string v = appVersion.Major.ToString() + "." +
          appVersion.Minor.ToString() + "." +
          appVersion.Build.ToString() + "." +
          appVersion.Revision.ToString();
            return v;
        }



        /// <summary>
        /// デバッグ用コンソールから呼び出すコマンド解析メソッド
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public async Task<string> DebugMethod(object o)
        {
            var tb = o as TextBox;
            var text = tb.Text;
            var cmd = text.Split(' ');
            try
            {

                return "avairable commands : " +
                       "\n    - qr [qr string]" +
                       "\n    - order [order number] " +
                       "\n    - torque [min] [max]" +
                       "\n    - trials [num]" +
                       "\n    or Press esc to quit";

            }
            catch
            {
                return "invalid command was given.";
            }


            return string.Empty;
        }



        public async void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {

            }
            else if (e.Key == Key.F2)
            {


            }
            else if (e.Key is Key.F11)
            {
                var console = new Views.Panels.DebugConsole();
                console.Show();

            }
            else if (e.Key == Key.F12)
            {



            }
        }


    }
}
