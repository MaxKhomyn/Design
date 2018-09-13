using System.Globalization;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows;
using System.Linq;
using System;

namespace Design.Converters
{
    public class BrushTranslationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(DP => DP == DependencyProperty.UnsetValue || DP == null)) return new Point(0, 0);
            
            //var pointerPos = (Point)values[2];
            var relativePos = (values[0] as UIElement).TranslatePoint(new Point(0, 0), (values[1] as UIElement));

            return new TranslateTransform(relativePos.X, relativePos.Y);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}