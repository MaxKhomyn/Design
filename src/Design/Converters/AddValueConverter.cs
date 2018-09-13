using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System;

namespace Design.Converters
{
    public class AddValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double sum = 0;
            foreach (var Value in values)
            {
                if (Value == DependencyProperty.UnsetValue) continue;                
                sum += (double)Value;
            }
            return sum;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}