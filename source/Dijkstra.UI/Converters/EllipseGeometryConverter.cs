using Dijkstra.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Dijkstra.UI.Converters
{
    public class EllipseGeometryConverter : MarkupExtension, IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GeometryCollection geoColl = new GeometryCollection();
            var sourceCollection = value as ICollection<Location>;
            if (sourceCollection != null)
            {
                int size = int.Parse(parameter.ToString());
                foreach (var node in sourceCollection)
                {
                    var nodeCenter = new Point(node.X * size, node.Y * size);
                    var geo = new EllipseGeometry(center: nodeCenter, radiusX: 2, radiusY: 2);
                    geoColl.Add(geo);
                }
            }
            return geoColl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
