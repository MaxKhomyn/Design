using System.Windows;

namespace Design.Model
{
    public static class PointerTracker
    {
        #region DependencyPropertys

        #region X

        public static readonly DependencyProperty XProperty = DependencyProperty.RegisterAttached
        (
            "X", typeof(double), typeof(PointerTracker),
            new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.Inherits)
        );

        public static double GetX(DependencyObject obj) => (double)obj.GetValue(XProperty);
        private static void SetX(DependencyObject obj, double value) => obj.SetValue(XProperty, value);

        #endregion

        #region Y

        public static readonly DependencyProperty YProperty = DependencyProperty.RegisterAttached
        (
            "Y", typeof(double), typeof(PointerTracker),
            new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.Inherits)
        );

        public static double GetY(DependencyObject obj) => (double)obj.GetValue(YProperty);
        private static void SetY(DependencyObject obj, double value) => obj.SetValue(YProperty, value);

        #endregion

        #region IsEnter

        public static readonly DependencyProperty IsEnterProperty = DependencyProperty.RegisterAttached
        ("IsEnter", typeof(bool), typeof(PointerTracker),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits)
        );

        public static bool GetIsEnter(DependencyObject obj) => (bool)obj.GetValue(IsEnterProperty);
        private static void SetIsEnter(DependencyObject obj, bool value) => obj.SetValue(IsEnterProperty, value);

        #endregion

        #region Enabled

        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached
        (
            "Enabled", typeof(bool), typeof(PointerTracker),
            new PropertyMetadata(false, OnEnabledChanged)
        );

        public static bool GetEnabled(DependencyObject obj) => (bool)obj.GetValue(EnabledProperty);
        public static void SetEnabled(DependencyObject obj, bool value) => obj.SetValue(EnabledProperty, value);

        #endregion

        #region Position

        public static readonly DependencyProperty PositionProperty = DependencyProperty.RegisterAttached
        (
            "Position", typeof(Point), typeof(PointerTracker),
            new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.Inherits)
        );

        public static Point GetPosition(DependencyObject obj) => (Point)obj.GetValue(PositionProperty);
        private static void SetPosition(DependencyObject obj, Point value) => obj.SetValue(PositionProperty, value);

        #endregion

        #region RootObject

        public static readonly DependencyProperty RootObjectProperty = DependencyProperty.RegisterAttached
        (
            "RootObject", typeof(UIElement), typeof(PointerTracker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
        );

        public static UIElement GetRootObject(DependencyObject obj) => (UIElement)obj.GetValue(RootObjectProperty);
        private static void SetRootObject(DependencyObject obj, UIElement value) => obj.SetValue(RootObjectProperty, value);

        #endregion

        #endregion

        #region Events

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var Ctrl = d as UIElement;
            if (Ctrl is null) return;

            var NewValue = (bool)e.NewValue;
            var OldValue = (bool)e.OldValue;
            
            #region Processing in case of invalidation

            if (OldValue && !NewValue)
            {
                Ctrl.MouseEnter -= Ctrl_MouseEnter;
                Ctrl.PreviewMouseMove -= Ctrl_PreviewMouseMove;
                Ctrl.MouseLeave -= Ctrl_MouseLeave;

                Ctrl.ClearValue(PointerTracker.RootObjectProperty);
            }

            #endregion
            #region Processing when enabled

            if (!OldValue && NewValue)
            {
                Ctrl.MouseEnter += Ctrl_MouseEnter;
                Ctrl.PreviewMouseMove += Ctrl_PreviewMouseMove;
                Ctrl.MouseLeave += Ctrl_MouseLeave;

                SetRootObject(Ctrl, Ctrl);
            }

            #endregion
        }

        private static void Ctrl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var Ctrl = sender as UIElement;
            if (Ctrl is null) return;

            SetIsEnter(Ctrl, true);
        }
        private static void Ctrl_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var Ctrl = sender as UIElement;
            if (Ctrl != null && GetIsEnter(Ctrl))
            {
                var Position = e.GetPosition(Ctrl);

                SetX(Ctrl, Position.X);
                SetY(Ctrl, Position.Y);
                SetPosition(Ctrl, Position);
            }
        }
        private static void Ctrl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var Ctrl = sender as UIElement;
            if (Ctrl is null) return;

            SetIsEnter(Ctrl, false);
        }

        #endregion
    }
}