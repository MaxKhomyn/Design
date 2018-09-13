using System.Collections.ObjectModel;
using Services.PropertyChanged;
using Services.Serialization;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Linq;
using Services;
using System;

namespace Design.Services
{
    public class ThemeService : StaticImplementationPropertyChanged
    {
        #region Fields
        
        private static ObservableCollection<string> _Themes = new ObservableCollection<string>() { "Light", "Dark" };
        public static string _SelectedTheme;

        #endregion Fields

        #region Properties

        public static ObservableCollection<string> Themes => _Themes;

        public static string SelectedTheme
        {
            get => _SelectedTheme;
            set
            {
                if (value.Equals(Thread.CurrentThread.CurrentUICulture) || value is null) return;

                _SelectedTheme = value;

                Properties.Settings.Default.DefaultTheme = value;
                Properties.Settings.Default.Save();

                ReplaceDictionary();
                SaveTheme();
                Static_OnPropertyChanged("SelectedTheme");
            }
        }

        #endregion Properties

        #region Constructor

        public ThemeService() { GetTheme(); }

        #endregion Constructor

        #region Methods

        private static void ReplaceDictionary() => ReplaceDictionary(SelectedTheme);
        private static void ReplaceDictionary(string Theme)
        {
            ResourceDictionary RD = new ResourceDictionary();
            switch (Theme)
            {
                case "en-US":
                {
                    RD.Source = new Uri
                    (
                        String.Format("/Design;component/Themes/Theme.{0}.xaml", Theme),
                        UriKind.Relative
                    );
                    break;
                }
                default:
                {
                    RD.Source = new Uri
                    (
                        String.Format("/Design;component/Themes/Theme.{0}.xaml", Theme),
                        UriKind.Relative
                    );
                    break;
                }
            }

            ResourceDictionary OldRD = null;
            try
            {
                OldRD = Application.Current.Resources.MergedDictionaries.First
                (
                    rd => rd.Source != null &&
                    rd.Source.OriginalString.StartsWith("pack://application:,,,/Design;component/Themes/Theme.")
                );
            }
            catch (Exception e) { ExceptionService.WriteLine(e.Message); }

            Collection<ResourceDictionary> Dictionaries = Application.Current.MainWindow.Resources.MergedDictionaries;

            switch (OldRD)
            {
                case null: { Dictionaries.Add(RD); break; }
                default:
                {
                    int OldRDIndex = Dictionaries.IndexOf(OldRD);
                    Dictionaries.Remove(OldRD);
                    Dictionaries.Insert(OldRDIndex, RD);
                    break;
                }
            }
        }

        private static void SaveTheme() => SaveTheme(SelectedTheme);
        private static void SaveTheme(string Theme)
        {
            JSONSerialization<string> JSON = new JSONSerialization<string>(Theme);
            JSON.CreateModel(DirectoryService.FullPath, "Theme");
        }

        private static void GetTheme()
        {
            JSONSerialization<string> JSON = new JSONSerialization<string>();

            switch (JSON.GetModel(DirectoryService.FullPath, "Theme"))
            {
                case true: SelectedTheme = JSON._Model; break;
                case false: SelectedTheme = Themes[0]; break;
            }
        }

        #endregion Methods

        #region Events



        #endregion Events
    }
}