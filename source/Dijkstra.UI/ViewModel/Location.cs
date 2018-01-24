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
    public class Location : ObservableObject
    {

        public Location(double x, double y)
        {
            Y = y;
            X = x;
            NetworkNode = new Node();
        }

        public int Id
        {
            get { return this.NetworkNode.Id; }
        }

        private double _Y = 0;
        public double Y
        {
            get { return _Y; }
            set { Set(() => Y, ref _Y, value); }
        }

        private double _X = 0;
        public double X
        {
            get { return _X; }
            set { Set(() => X, ref _X, value); }
        }

        public Node NetworkNode { get; private set; }

    }
}
