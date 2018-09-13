using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System;

namespace Design.Resources
{
    public enum ApplicationTheme { Light, Dark, }

    public class SystemTheme : ThemeHandler
    {
        #region Fields_Properties

        private static readonly int WM_WININICHANGE = 0x001A;
        private static readonly int WM_SETTINGCHANGE = WM_WININICHANGE;

        public static event EventHandler ThemeChanged;

        private static ApplicationTheme theme;
        public static ApplicationTheme Theme
        {
            get { return theme; }
            private set { if (!object.Equals(theme, value)) { theme = value; OnStaticPropertyChanged(); } }
        }

        #endregion Fields_Properties

        #region Constructor

        static SystemTheme()
        {
            SystemTheme.Instance = new SystemTheme();
            Theme = GetTheme();
        }
        #endregion Constructor

        #region Methods

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SETTINGCHANGE)
            {
                var systemParmeter = Marshal.PtrToStringAuto(lParam);
                if (systemParmeter == "ImmersiveColorSet")
                {
                    Theme = GetTheme();
                    SystemTheme.ThemeChanged?.Invoke(null, null);

                    handled = true;
                }
            }

            return IntPtr.Zero;
        }

        private static ApplicationTheme GetTheme()
        {
            var regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", false);
            if (regkey == null) return ApplicationTheme.Light;
            var intValue = (int)regkey.GetValue("AppsUseLightTheme", 1);

            return intValue == 0 ? ApplicationTheme.Dark : ApplicationTheme.Light;
        }
        
        #endregion Methods

        #region Events

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        protected static void OnStaticPropertyChanged([CallerMemberName]string propertyName = null) => StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));

        #endregion Events
    }
}