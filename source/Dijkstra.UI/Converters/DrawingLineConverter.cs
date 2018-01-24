using Dijkstra.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Dijkstra.UI.Converters
{
    public class DrawingLineConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GeometryCollection geoColl = new GeometryCollection();
            var sourceCollection = value as ICollection<Connection>;
            if (sourceCollection != null)
            {
                int size = int.Parse(parameter.ToString());
                foreach (var arc in sourceCollection)
                {
                    var originPoint = new Point(x: arc.OriginLocation.X * size, y: arc.OriginLocation.Y * size);
                    var destinationPoint = new Point(x: arc.DestinationLocation.X * size, y: arc.DestinationLocation.Y * size);
                    var geo = new LineGeometry(originPoint, destinationPoint);
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

        private static Brush LineBrush = new SolidColorBrush(Colors.Black);
        private static Pen LinePen = new Pen(LineBrush, 1);
    }
}
