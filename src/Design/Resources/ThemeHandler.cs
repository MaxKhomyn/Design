using System.Windows.Interop;
using System.Windows;
using System;

namespace Design.Resources
{
    public abstract class ThemeHandler
    {
        #region Fields

        protected static ThemeHandler Instance { get; set; }

        #endregion Fields

        #region Constructors

        static ThemeHandler() { }

        public ThemeHandler()
        {
            if (Application.Current.MainWindow is null)
            {
                EventHandler handler = null;
                handler = (e, args) =>
                {
                    this.Initialize(Application.Current.MainWindow);
                    Application.Current.Activated -= handler;
                };
                Application.Current.Activated += handler;
            }
            else this.Initialize(Application.Current.MainWindow);
        }

        #endregion Constructors

        #region Methods

        private void Initialize(Window win)
        {
            if (win.IsLoaded) InitializeCore(win);
            else
                win.Loaded += (_, __) =>
                {
                    InitializeCore(win);
                };
        }

        protected virtual void InitializeCore(Window win)
        {
            var source = HwndSource.FromHwnd(new WindowInteropHelper(win).Handle);
            source.AddHook(this.WndProc);
        }

        protected abstract IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);

        #endregion Methods
    }
}