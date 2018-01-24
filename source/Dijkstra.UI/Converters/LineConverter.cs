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
    public class LineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var c = (Connection)value;
            if (c == null)
            {
                return null;
            }

            var pc = new PointCollection();
            pc.Add(new Point(c.OriginLocation.X,c.OriginLocation.Y));
            pc.Add(new Point(c.DestinationLocation.X, c.DestinationLocation.Y));
            return pc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
