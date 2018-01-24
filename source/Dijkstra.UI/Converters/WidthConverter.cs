using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Dijkstra.UI.Converters
{
    public class WidthConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == DependencyProperty.UnsetValue) return null;
            }
            double actualWidthOrHeight = (double)value[0];
            double normalizedWidthOrHeight = (double)value[1];
            return (normalizedWidthOrHeight * actualWidthOrHeight);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
