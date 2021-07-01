using ControlzEx.Theming;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImagePasterForExcel.Model
{
    public class ThemeSelector
    {

        public IEnumerable<Swatch> Swatches { get; set; } = new SwatchesProvider().Swatches;


        public static bool IsDarkTheme { get; set; }


        public static string PrimaryColorCode { get; set; }

        public static string PrimaryColor { get; set; }

        public static string AccentColorCode { get; set; }

        public static void ApplyPrimary(Swatch swatch)
        {

            /*============================================================================*
             * material design の theme 変更
            /*============================================================================*/
            ModifyTheme(_ => _.SetPrimaryColor(swatch.ExemplarHue.Color));
            PrimaryColorCode = swatch.ExemplarHue.Color.ToString();

            /*============================================================================*
             * MahApps の theme変更
            /*============================================================================*/
            var color = swatch.Name;
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            var mode = theme.GetBaseTheme() == BaseTheme.Dark ? "Dark" : "Light";
            try
            {
                switch (color)
                {
                    case "lightblue":
                        color = "Blue";
                        break;
                    case "deeppurple":
                        color = "Purple";
                        break;
                    case "purple":
                        color = "Violet";
                        break;

                    case "lightgreen":
                        color = "Green";
                        break;
                    case "deeporange":
                        color = "Orange";
                        break;
                    case "bluegrey":
                        color = "Steel";
                        break;
                    case "grey":
                        color = "Steel";
                        break;

                };


                ThemeManager.Current.ChangeTheme(Application.Current, $"{mode}.{color}");
            }
            catch
            {
                // ignored
            }

            PrimaryColor = color;

        }

        public static void ApplyAccent(Swatch swatch)
        {
            if (swatch?.AccentExemplarHue is null) return;

            ModifyTheme(theme => theme.SetSecondaryColor(swatch.AccentExemplarHue.Color));
            AccentColorCode = swatch.AccentExemplarHue.Color.ToString();
        }
        public static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var ph = new PaletteHelper();
            var theme = ph.GetTheme();
            modificationAction?.Invoke(theme);
            ph.SetTheme(theme);
        }

    }
}
