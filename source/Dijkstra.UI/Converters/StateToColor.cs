using Dijkstra.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Dijkstra.UI.Converters
{
    public class StateToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CellState c = (CellState)value;
            Brush b;
            switch (c)
            {
                case CellState.IsActive:
                    b = new SolidColorBrush(Colors.Goldenrod);
                    break;
                case CellState.IsInactive:
                    b = new SolidColorBrush(Colors.Black);
                    break;
                case CellState.IsSelected:
                    b = new SolidColorBrush(Colors.IndianRed);
                    break;
                case CellState.IsPath:
                    b = new SolidColorBrush(Colors.LightSteelBlue);
                    break;
                default:
                    b = new SolidColorBrush(Colors.SteelBlue);
                    break;
            }
            return b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
