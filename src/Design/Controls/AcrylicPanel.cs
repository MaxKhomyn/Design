using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows;
using System;

namespace Design.Controls
{
    public class AcrylicPanel : ContentControl
    {
        #region Fields
        
        bool _isChanged = false;

        #endregion

        #region DependencyPropertys

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register
        (
            "Target", typeof(FrameworkElement),
            typeof(AcrylicPanel), new PropertyMetadata(null)
        );
        public FrameworkElement Target
        {
            get { return (FrameworkElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register
        (
            "Source", typeof(FrameworkElement),
            typeof(AcrylicPanel), new PropertyMetadata(null)
        );
        public FrameworkElement Source
        {
            get { return (FrameworkElement)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        
        public static readonly DependencyProperty TintColorProperty = DependencyProperty.Register
        (
            "TintColor", typeof(Color),
            typeof(AcrylicPanel), new PropertyMetadata(Colors.White)
        );
        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }
        
        public static readonly DependencyProperty TintOpacityProperty = DependencyProperty.Register
        (
            "TintOpacity", typeof(double),
            typeof(AcrylicPanel), new PropertyMetadata(0.0)
        );
        public double TintOpacity
        {
            get { return (double)GetValue(TintOpacityProperty); }
            set { SetValue(TintOpacityProperty, value); }
        }
        
        public static readonly DependencyProperty NoiseOpacityProperty = DependencyProperty.Register
        (
            "NoiseOpacity", typeof(double),
            typeof(AcrylicPanel), new PropertyMetadata(0.03)
        );
        public double NoiseOpacity
        {
            get { return (double)GetValue(NoiseOpacityProperty); }
            set { SetValue(NoiseOpacityProperty, value); }
        }

        #endregion

        #region Constructors

        static AcrylicPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AcrylicPanel), new FrameworkPropertyMetadata(typeof(AcrylicPanel)));
        }
        public AcrylicPanel()
        {
            this.Source = this;
        }

        #endregion

        #region Events

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var rect = this.GetTemplateChild("rect") as Rectangle;
            if (rect != null)
            {
                rect.LayoutUpdated += (_, __) =>
                {
                    if (!this._isChanged)
                    {
                        this._isChanged = true;
                        BindingOperations.GetBindingExpressionBase(rect, Rectangle.RenderTransformProperty)?.UpdateTarget();

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this._isChanged = false;
                        }), System.Windows.Threading.DispatcherPriority.DataBind);
                    }
                };
            }
        }

        #endregion
    }
}