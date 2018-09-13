using System.Windows.Input;
using System.Windows.Media;
using Design.CustomColors;
using System.Management;
using Microsoft.Win32;
using System.Windows;
using System.Linq;
using System;

namespace Design.Controls
{
    public class AcrylicWindow : Window
    {
        #region Propertys

        public bool TabletMode => (int)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ImmersiveShell", "TabletMode", 0) == 1 ? true : false;

        #endregion Propertys

        #region DependencyPropertys

        #region TintColor

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        public static Color GetTintColor(DependencyObject obj) => (Color)obj.GetValue(AcrylicElement.TintColorProperty);
        public static void SetTintColor(DependencyObject obj, Color value) => obj.SetValue(AcrylicElement.TintColorProperty, value);

        public static readonly DependencyProperty TintColorProperty = AcrylicElement.TintColorProperty.AddOwner(typeof(AcrylicWindow), new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.Inherits));

        #endregion TintColor

        #region TintOpacity

        public double TintOpacity
        {
            get { return (double)GetValue(TintOpacityProperty); }
            set { SetValue(TintOpacityProperty, value); }
        }

        public static double GetTintOpacity(DependencyObject obj) => (double)obj.GetValue(AcrylicElement.TintOpacityProperty);
        public static void SetTintOpacity(DependencyObject obj, double value) => obj.SetValue(AcrylicElement.TintOpacityProperty, value);

        public static readonly DependencyProperty TintOpacityProperty = AcrylicElement.TintOpacityProperty.AddOwner(typeof(AcrylicWindow), new FrameworkPropertyMetadata(0.6, FrameworkPropertyMetadataOptions.Inherits));

        #endregion TintOpacity

        #region NoiseOpacity

        public double NoiseOpacity
        {
            get { return (double)GetValue(NoiseOpacityProperty); }
            set { SetValue(NoiseOpacityProperty, value); }
        }

        public static double GetNoiseOpacity(DependencyObject obj) => (double)obj.GetValue(AcrylicElement.NoiseOpacityProperty);
        public static void SetNoiseOpacity(DependencyObject obj, double value) => obj.SetValue(AcrylicElement.NoiseOpacityProperty, value);

        public static readonly DependencyProperty NoiseOpacityProperty = AcrylicElement.NoiseOpacityProperty.AddOwner(typeof(AcrylicWindow), new FrameworkPropertyMetadata(0.03, FrameworkPropertyMetadataOptions.Inherits));

        #endregion NoiseOpacity

        #region FallbackColor

        public Color FallbackColor
        {
            get { return (Color)GetValue(FallbackColorProperty); }
            set { SetValue(FallbackColorProperty, value); }
        }

        public static Color GetFallbackColor(DependencyObject obj) => (Color)obj.GetValue(AcrylicElement.FallbackColorProperty);
        public static void SetFallbackColor(DependencyObject obj, Color value) => obj.SetValue(AcrylicElement.FallbackColorProperty, value);

        public static readonly DependencyProperty FallbackColorProperty = AcrylicElement.FallbackColorProperty.AddOwner(typeof(AcrylicWindow), new FrameworkPropertyMetadata(Colors.LightGray, FrameworkPropertyMetadataOptions.Inherits));

        #endregion NoiseOpacity

        #region ShowTitleBar

        public bool ShowTitleBar
        {
            get { return (bool)GetValue(ShowTitleBarProperty); }
            set { SetValue(ShowTitleBarProperty, value); }
        }
        
        public static bool GetShowTitleBar(DependencyObject obj) => (bool)obj.GetValue(AcrylicElement.ShowTitleBarProperty);
        public static void SetShowTitleBar(DependencyObject obj, bool value) => obj.SetValue(AcrylicElement.ShowTitleBarProperty, value);

        public static readonly DependencyProperty ShowTitleBarProperty = AcrylicElement.ShowTitleBarProperty.AddOwner(typeof(AcrylicWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

        #endregion ShowTitleBar

        #region ExtendViewIntoTitleBar

        public bool ExtendViewIntoTitleBar
        {
            get { return (bool)GetValue(ExtendViewIntoTitleBarProperty); }
            set { SetValue(ExtendViewIntoTitleBarProperty, value); }
        }

        public static bool GetExtendViewIntoTitleBar(DependencyObject obj) => (bool)obj.GetValue(AcrylicElement.ExtendViewIntoTitleBarProperty);
        public static void SetExtendViewIntoTitleBar(DependencyObject obj, bool value) => obj.SetValue(AcrylicElement.ExtendViewIntoTitleBarProperty, value);

        public static readonly DependencyProperty ExtendViewIntoTitleBarProperty = AcrylicElement.ExtendViewIntoTitleBarProperty.AddOwner(typeof(AcrylicWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        #endregion ShowTitleBar

        #region IsTabletMode

        public bool IsTabletMode
        {
            get { return (bool)GetValue(IsTabletModeProperty); }
            set { SetValue(IsTabletModeProperty, value); }
        }

        public static bool GetIsTabletMode(DependencyObject obj) => (bool)obj.GetValue(IsTabletModeProperty);
        public static void SetIsTabletMode(DependencyObject obj, bool value) => obj.SetValue(IsTabletModeProperty, value);

        public static readonly DependencyProperty IsTabletModeProperty = DependencyProperty.Register("IsTabletMode", typeof(bool),
            typeof(AcrylicWindow), new PropertyMetadata(true, null));

        #endregion ShowTitleBar

        #endregion DependencyPropertys

        #region Attached Property

        #region Enabled

        public static bool GetEnabled(DependencyObject obj) => (bool)obj.GetValue(EnabledProperty);
        public static void SetEnabled(DependencyObject obj, bool value) => obj.SetValue(EnabledProperty, value);
        
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached
        (
            "Enabled", typeof(bool), typeof(AcrylicWindow),
            new PropertyMetadata(false, OnEnableChanged)
        );

        #endregion Enabled

        #endregion Attached Property

        #region Constructor

        static AcrylicWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AcrylicWindow), new FrameworkPropertyMetadata(typeof(AcrylicWindow)));
        }

        #endregion Constructor

        #region Methods

        internal static bool IsWindows10()
        {
            bool IsWindows10 = false;
            using (ManagementClass MC = new ManagementClass("Win32_OperatingSystem"))
            {
                using (ManagementObjectCollection MOC = MC.GetInstances())
                {
                    foreach (ManagementObject MO in MOC)
                    {
                        IsWindows10 = ((MO["Version"] as string).Split('.').FirstOrDefault()) == "10";
                    }
                }
            }
            return IsWindows10;
        }

        internal static void AddCommandBindings(Window window)
        {
            window.CommandBindings.Add
            (
                new CommandBinding(SystemCommands.CloseWindowCommand, (_, __) =>
                { SystemCommands.CloseWindow(window); })
            );
            window.CommandBindings.Add
            (
                new CommandBinding(SystemCommands.MinimizeWindowCommand, (_, __) =>
                { SystemCommands.MinimizeWindow(window); })
            );
            window.CommandBindings.Add
            (
                new CommandBinding(SystemCommands.MaximizeWindowCommand, (_, __) =>
                { SystemCommands.MaximizeWindow(window); })
            );
            window.CommandBindings.Add
            (
                new CommandBinding(SystemCommands.RestoreWindowCommand, (_, __) =>
                { SystemCommands.RestoreWindow(window); })
            );
        }

        #endregion Methods  

        #region Events

        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window is null) return;

            if ((bool)e.NewValue)
            {
                var dic = new ResourceDictionary() { Source = new Uri("pack://application:,,,/Design;component/Styles/Forms/Window.xaml") };
                window.Style = dic["AcrylicWindowStyle"] as Style;

                window.Loaded += (_, __) =>
                {
                    Blur.Enable(window);
                    AddCommandBindings(window);
                };
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Blur.Enable(this);
            AddCommandBindings(this);
        }

        #endregion Events
    }
}