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
    public class LanguageService : StaticImplementationPropertyChanged
    {
        #region Fields

        public static event EventHandler LanguageChanged;

        private static ObservableCollection<CultureInfo> _Languages = new ObservableCollection<CultureInfo>()
        {
            new CultureInfo("uk-UA"),
            new CultureInfo("en-US")
        };
        public static CultureInfo _SelectedLanguage;
        
        #endregion Fields

        #region Properties
        
        public static ObservableCollection<CultureInfo> Languages => _Languages;

        public static CultureInfo SelectedLanguage
        {
            get => Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value.Equals(Thread.CurrentThread.CurrentUICulture) || value is null) return;

                _SelectedLanguage = value;
                Thread.CurrentThread.CurrentUICulture = value;
                
                Properties.Settings.Default.DefaultLanguage = value;
                Properties.Settings.Default.Save();

                ReplaceDictionary();
                SaveLanguage();
                Static_OnPropertyChanged("SelectedLanguage");
            }
        }

        #endregion Properties

        #region Constructor

        public LanguageService() { GetLanguage(); }

        #endregion Constructor

        #region Methods

        private static void ReplaceDictionary() => ReplaceDictionary(SelectedLanguage);
        private static void ReplaceDictionary(CultureInfo Language)
        {
            ResourceDictionary RD = new ResourceDictionary();
            switch (Language.Name)
            {
                case "en-US":
                {
                    RD.Source = new Uri
                    (
                        String.Format("/Design;component/Localization/lang.{0}.xaml", Language.Name),
                        UriKind.Relative
                    );
                    break;
                }
                default:
                {
                    RD.Source = new Uri
                    (
                        String.Format("/Design;component/Localization/lang.xaml"),
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
                    rd.Source.OriginalString.StartsWith("pack://application:,,,/Design;component/Localization/lang.")
                );
            }
            catch (Exception e) { ExceptionService.WriteLine(e.Message); }

            Collection<ResourceDictionary> Dictionaries = Application.Current.MainWindow.Resources.MergedDictionaries;

            switch (OldRD)
            {
                case null:
                {
                    Dictionaries.Add(RD);
                    break;
                }
                default:
                {
                    int OldRDIndex = Dictionaries.IndexOf(OldRD);
                    Dictionaries.Remove(OldRD);
                    Dictionaries.Insert(OldRDIndex, RD);
                    break;
                }
            }

            LanguageChanged(Application.Current, new EventArgs());
        }

        private static void SaveLanguage() => SaveLanguage(SelectedLanguage);
        private static void SaveLanguage(CultureInfo Language)
        {
            JSONSerialization<CultureInfo> JSON = new JSONSerialization<CultureInfo>(Language);
            JSON.CreateModel(DirectoryService.FullPath, "Language");
        }
        
        private static void GetLanguage()
        {
            JSONSerialization<CultureInfo> JSON = new JSONSerialization<CultureInfo>();

            switch (JSON.GetModel(DirectoryService.FullPath, "Language"))
            {
                case true: SelectedLanguage = JSON._Model; break;
                case false: SelectedLanguage = Properties.Settings.Default.DefaultLanguage; break;
            }
        }

        #endregion Methods

        #region Events



        #endregion Events
    }
}