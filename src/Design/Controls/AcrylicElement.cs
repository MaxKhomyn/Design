using System.Windows.Media;
using System.Windows;

namespace Design.Controls
{
    internal class AcrylicElement
    {
        #region DependencyPropertys

        #region TintColor

        public static readonly DependencyProperty TintColorProperty = DependencyProperty.RegisterAttached
        (
            "TintColor", typeof(Color), typeof(AcrylicElement),
            new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.Inherits)
        );
        public static Color GetTintColor(DependencyObject obj) => (Color)obj.GetValue(TintColorProperty);
        public static void SetTintColor(DependencyObject obj, Color value) => obj.SetValue(TintColorProperty, value);

        #endregion
        #region TintOpacity

        public static readonly DependencyProperty TintOpacityProperty = DependencyProperty.RegisterAttached
        (
            "TintOpacity", typeof(double),
            typeof(AcrylicElement), new PropertyMetadata(0.6)
        );
        public static double GetTintOpacity(DependencyObject obj) => (double)obj.GetValue(TintOpacityProperty);
        public static void SetTintOpacity(DependencyObject obj, double value) => obj.SetValue(TintOpacityProperty, value);

        #endregion
        #region NoiseOpacity
        public static readonly DependencyProperty NoiseOpacityProperty = DependencyProperty.RegisterAttached
        (
            "NoiseOpacity", typeof(double),
            typeof(AcrylicElement), new PropertyMetadata(0.03)
        );
        public static double GetNoiseOpacity(DependencyObject obj) => (double)obj.GetValue(NoiseOpacityProperty);
        public static void SetNoiseOpacity(DependencyObject obj, double value) => obj.SetValue(NoiseOpacityProperty, value);

        #endregion
        #region ShowTitleBar
        public static readonly DependencyProperty ShowTitleBarProperty = DependencyProperty.RegisterAttached
        (
            "ShowTitleBar", typeof(bool),
            typeof(AcrylicElement), new PropertyMetadata(true)
        );
        public static bool GetShowTitleBar(DependencyObject obj) => (bool)obj.GetValue(ShowTitleBarProperty);
        public static void SetShowTitleBar(DependencyObject obj, bool value) => obj.SetValue(ShowTitleBarProperty, value);

        #endregion
        #region FallbackColor
        public static readonly DependencyProperty FallbackColorProperty = DependencyProperty.RegisterAttached
        (
            "FallbackColor", typeof(Color),
            typeof(AcrylicElement), new PropertyMetadata(Colors.LightGray)
        );
        public static Color GetFallbackColor(DependencyObject obj) => (Color)obj.GetValue(FallbackColorProperty);
        public static void SetFallbackColor(DependencyObject obj, Color value) => obj.SetValue(FallbackColorProperty, value);

        #endregion
        #region ExtendViewIntoTitleBar

        public static readonly DependencyProperty ExtendViewIntoTitleBarProperty = DependencyProperty.RegisterAttached
        (
            "ExtendViewIntoTitleBar", typeof(bool),
            typeof(AcrylicElement), new PropertyMetadata(false)
        );
        public static bool GetExtendViewIntoTitleBar(DependencyObject obj) => (bool)obj.GetValue(ExtendViewIntoTitleBarProperty);
        public static void SetExtendViewIntoTitleBar(DependencyObject obj, bool value) => obj.SetValue(ExtendViewIntoTitleBarProperty, value);

        #endregion

        #endregion
    }
}