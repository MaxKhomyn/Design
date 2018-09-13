using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System;

namespace Design.Converters
{
    internal class NegativeMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => new Thickness(-(double)value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}