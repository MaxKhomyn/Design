using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Data;
using Design.Converters;
using System.Windows;
using System;

namespace Design.Model
{
    public class RevealBrushExtension : MarkupExtension
    {
        #region Properties

        public Color Color { get; set; } = Colors.Dark;
        public double Opacity { get; set; } = 1;
        public double Size { get; set; } = 100;

        #endregion

        #region Constructor

        public RevealBrushExtension() { }

        #endregion

        #region Methods

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var dst = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var target = dst.TargetObject as FrameworkElement;

            #region Create a circular gradient display brush

            var bgColor = Color.FromArgb(0, this.Color.R, this.Color.G, this.Color.B);
            var brush = new RadialGradientBrush(this.Color, bgColor);
            brush.MappingMode = BrushMappingMode.Absolute;
            brush.RadiusX = this.Size;
            brush.RadiusY = this.Size;

            #endregion
            #region When the cursor is outside the area, make it transparent.

            var opacityBinding = new Binding("Opacity")
            {
                Source = dst.TargetObject,
                Path = new PropertyPath(PointerTracker.IsEnterProperty),
                Converter = new OpacityConverter(),
                ConverterParameter = this.Opacity
            };
            BindingOperations.SetBinding(brush, RadialGradientBrush.OpacityProperty, opacityBinding);

            #endregion
            #region Binding the center position of the gradation

            var binding = new MultiBinding();
            binding.Converter = new RelativePositionConverter();
            binding.Bindings.Add(new Binding() { Source = dst.TargetObject, Path = new PropertyPath(PointerTracker.RootObjectProperty) });
            binding.Bindings.Add(new Binding() { Source = dst.TargetObject });
            binding.Bindings.Add(new Binding() { Source = dst.TargetObject, Path = new PropertyPath(PointerTracker.PositionProperty) });

            BindingOperations.SetBinding(brush, RadialGradientBrush.CenterProperty, binding);
            BindingOperations.SetBinding(brush, RadialGradientBrush.GradientOriginProperty, binding);

            #endregion

            return brush;
        }

        #endregion
    }
}