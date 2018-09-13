using System.Windows;
using System.Linq;
using System;

namespace Design.Resources
{
    public enum ElementTheme { Default, Light, Dark, }

    public class ResourceDictionaryEx : ResourceDictionary
    {
        #region Fields_Properties

        #region StaticMembers

        private static ElementTheme? globalTheme;
        public static ElementTheme? GlobalTheme
        {
            get => globalTheme;
            set { globalTheme = value; GlobalThemeChanged?.Invoke(null, null); }
        }

        public static event EventHandler<EventArgs> GlobalThemeChanged;
        
        #endregion StaticMembers

        public ThemeCollection ThemeDictionaries { get; set; } = new ThemeCollection();

        private ElementTheme? requestedTheme;
        public ElementTheme? RequestedTheme
        {
            get => requestedTheme;
            set { requestedTheme = value; this.ChangeTheme(); }
        }

        #endregion Fields_Properties

        #region Constructor

        public ResourceDictionaryEx()
        {
            SystemTheme.ThemeChanged += SystemTheme_ThemeChanged;
            this.ThemeDictionaries.CollectionChanged += ThemeDictionaries_CollectionChanged;
            ResourceDictionaryEx.GlobalThemeChanged += ResourceDictionaryEx_GlobalThemeChanged;
        }

        #endregion Constructor

        #region Methods

        private void SystemTheme_ThemeChanged(object sender, EventArgs e) => ChangeTheme();

        private void ThemeDictionaries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => ChangeTheme();

        private void ResourceDictionaryEx_GlobalThemeChanged(object sender, EventArgs e) => ChangeTheme();
        
        private void ChangeTheme()
        {
            var theme = this.RequestedTheme ?? ResourceDictionaryEx.GlobalTheme;
            switch (theme)
            {
                case ElementTheme.Light:
                    this.ChangeTheme(ApplicationTheme.Light.ToString());
                    break;
                case ElementTheme.Dark:
                    this.ChangeTheme(ApplicationTheme.Dark.ToString());
                    break;
                case ElementTheme.Default:
                default:
                    this.ChangeTheme(SystemTheme.Theme.ToString());
                    break;
            }
        }

        private void ChangeTheme(string themeName)
        {
            this.MergedDictionaries.Clear();

            ThemeDictionary theme = this.ThemeDictionaries.OfType<ThemeDictionary>().FirstOrDefault(o => o.ThemeName == themeName);
            if (theme != null) this.MergedDictionaries.Add(theme);
        }

        #endregion Methods
    }
}