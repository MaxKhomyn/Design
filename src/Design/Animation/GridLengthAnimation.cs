using System.Windows.Media.Animation;
using System.Windows;
using System;

namespace Design.Library.Animation
{
    public class GridLengthAnimation : AnimationTimeline
    {
        #region Propertys

        public GridLength From
        {
            get => (GridLength)GetValue(GridLengthAnimation.FromProperty);
            set => SetValue(GridLengthAnimation.FromProperty, value);
        }
        public GridLength To
        {
            get => (GridLength)GetValue(GridLengthAnimation.ToProperty);
            set => SetValue(GridLengthAnimation.ToProperty, value);
        }

        public override Type TargetPropertyType => typeof(GridLength);

        #endregion Propertys

        #region DependencyProperty

        public static readonly DependencyProperty FromProperty = DependencyProperty.Register
        (
            "From", typeof(GridLength), typeof(GridLengthAnimation)
        );
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register
        (
            "To", typeof(GridLength), typeof(GridLengthAnimation)
        );

        #endregion DependencyProperty

        #region Constructor

        static GridLengthAnimation() { }

        #endregion Constructor

        #region Methods

        protected override Freezable CreateInstanceCore() => new GridLengthAnimation();

        public override object GetCurrentValue
        (
            object defaultOriginValue,
            object defaultDestinationValue,
            AnimationClock animationClock
        )
        {
            double fromVal = ((GridLength)GetValue(FromProperty)).Value;
            double toVal = ((GridLength)GetValue(ToProperty)).Value;

            if (fromVal > toVal) return new GridLength((1 - animationClock.CurrentProgress.Value) * (fromVal - toVal) + toVal, GridUnitType.Pixel);
            else return new GridLength(animationClock.CurrentProgress.Value * (toVal - fromVal) + fromVal, GridUnitType.Pixel);
        }

        #endregion Methods
    }
}