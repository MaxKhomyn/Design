using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Linq;
using System;

namespace Design.Converters
{
    public class RelativePositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(DP => DP == DependencyProperty.UnsetValue || DP == null))
                return new Point(0, 0);
            
            return (values[0] as UIElement).TranslatePoint(((Point)values[2]), (values[1] as UIElement));
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}