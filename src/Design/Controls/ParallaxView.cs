using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Design.Controls
{
    public class ParallaxView : ContentControl
    {
        #region DependencyPropertys

        public static readonly DependencyProperty VerticalShiftProperty = DependencyProperty.Register
        (
            "VerticalShift", typeof(double),
            typeof(ParallaxView), new PropertyMetadata(0.0)
        );
        public double VerticalShift
        {
            get { return (double)GetValue(VerticalShiftProperty); }
            set { SetValue(VerticalShiftProperty, value); }
        }
        
        public static readonly DependencyProperty HorizontalShiftProperty = DependencyProperty.Register
        (
            "HorizontalShift", typeof(double),
            typeof(ParallaxView), new PropertyMetadata(0.0)
        );
        public double HorizontalShift
        {
            get { return (double)GetValue(HorizontalShiftProperty); }
            set { SetValue(HorizontalShiftProperty, value); }
        }
        
        public static readonly DependencyProperty OffsetMarginProperty = DependencyProperty.Register
        (
            "OffsetMargin", typeof(Thickness),
            typeof(ParallaxView), new PropertyMetadata(new Thickness(0))
        );
        public Thickness OffsetMargin
        {
            get { return (Thickness)GetValue(OffsetMarginProperty); }
            private set { SetValue(OffsetMarginProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register
        (
            "Source", typeof(UIElement),
            typeof(ParallaxView), new PropertyMetadata(null, OnSourceChanged)
        );
        public UIElement Source
        {
            get { return (UIElement)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        
        #endregion

        #region Constructor

        static ParallaxView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ParallaxView), new FrameworkPropertyMetadata(typeof(ParallaxView)));
        }

        #endregion

        #region Methods

        private static ScrollViewer GetScrollViewer(DependencyObject obj) => obj as ScrollViewer ?? FindVisualChild<ScrollViewer>(obj);

        private static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is childItem) return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        #endregion

        #region Events

        private void OnScrollUpdated(ScrollViewer scrollViewer)
        {
            var posX = scrollViewer.ScrollableWidth == 0 ? 0 : scrollViewer.HorizontalOffset / scrollViewer.ScrollableWidth;
            var posY = scrollViewer.ScrollableHeight == 0 ? 0 : scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight;

            this.OffsetMargin = new Thickness(-posX * HorizontalShift, -posY * VerticalShift, 0, 0);
        }
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // When Source is set, follow VisualTree and search for ScrollViewer.
            // Binding the various ScrollViewer properties found and the offset value of this ParallaxView.

            var Parallax = d as ParallaxView;
            var Ctrl = e.NewValue as FrameworkElement;

            Ctrl.Loaded += (_, __) =>
            {
                var viewer = GetScrollViewer(Ctrl);

                if (viewer != null)
                {
                    viewer.ScrollChanged += (sender, ___) => { Parallax.OnScrollUpdated(sender as ScrollViewer); };
                    viewer.SizeChanged += (sender, ___) => { Parallax.OnScrollUpdated(sender as ScrollViewer); };
                }
            };
        }

        #endregion
    }
}