using Dijkstra.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra.UI.Model
{
    public class NodeFinder
    {
        public static Location FindLocation(IEnumerable<Location> searchSet, double x, double y)
        {
            if (searchSet == null || !searchSet.Any())
                return null;

            double dist = double.PositiveInfinity;
            Location foundLocation = null;
            foreach (var node in searchSet)
            {
                var d = CalculateDistance(x, y, node);
                if(d < dist)
                {
                    dist = d;
                    foundLocation = node;
                }
            }
            return foundLocation;
        }

        private static double CalculateDistance(double x, double y, Location location)
        {
            return Math.Sqrt(Math.Pow(x - location.X, 2) + Math.Pow(y - location.Y, 2));
        }
    }
}
