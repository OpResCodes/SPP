using GalaSoft.MvvmLight;
using ShortestPaths;
using ShortestPaths.Dijkstra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra.UI.ViewModel
{
    public class Connection : ObservableObject
    {

        public Connection(Location origin, Location destination)
        {
            if (origin == null || destination == null)
            {
                throw new ArgumentNullException("Start or End Node");
            }
            NetworkArc = new Arc(origin.NetworkNode, destination.NetworkNode, CalculateDistance(origin, destination));
            NetworkArc.AutoAttach();
            OriginLocation = origin;
            DestinationLocation = destination;
        }

        public int Id
        {
            get
            {
                return NetworkArc.Id;
            }
        }

        public Arc NetworkArc { get; private set; }
                
        public Location OriginLocation
        {
            get { return _originLocation; }
            set { Set(() => OriginLocation, ref _originLocation, value); }
        }
                
        public Location DestinationLocation
        {
            get { return _destinationLocaion; }
            set { Set(() => DestinationLocation, ref _destinationLocaion, value); }
        }
        
        public double CalculateDistance(Location l1, Location l2)
        {
            double x_dev = Math.Pow(l1.X - l2.X,2.0);
            double y_dev = Math.Pow(l1.Y - l2.Y,2.0);
            double dist = Math.Sqrt(x_dev + y_dev);
            return dist;
        }

        private Location _originLocation = null;
        private Location _destinationLocaion = null;
        private bool _IsEmphasized = false;
        public bool IsEmphasized
        {
            get { return _IsEmphasized; }
            set { Set(() => IsEmphasized, ref _IsEmphasized, value); }
        }
    }
}
