using Dijkstra.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Dijkstra.UI.Converters
{
    public class WidthConverterLines : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            /*
            * [0] = width of canvas
            * [1] = height of canvas
            * [2] = node collection of tour
            */

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == DependencyProperty.UnsetValue) return null;
            }

            PointCollection result = new PointCollection();

            double actualWidth = (double)value[0];
            double actualHeight = (double)value[1];
            Location StartNode = (Location)value[2];
            Location EndNode = (Location)value[3];

            double areaX = StartNode.X * actualWidth;
            double areaY = StartNode.Y * actualHeight;
            result.Add(new Point(areaX, areaY));
            double areaX2 = EndNode.X * actualWidth;
            double areaY2 = EndNode.Y * actualHeight;
            result.Add(new Point(areaX2, areaY2));
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
