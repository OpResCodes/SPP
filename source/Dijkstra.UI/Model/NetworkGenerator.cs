using Dijkstra.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra.UI.Model
{
    public class NetworkGenerator
    {
        public NetworkGenerator() : this(30, 0.5) { }

        public NetworkGenerator(int numberOfNodes, double density)
        {
            NumberOfNodes = numberOfNodes;
            Density = density;
        }

        private readonly Random randomSource = new Random();

        public int NumberOfNodes { get; set; }

        public double Density { get; set; }

        private Location GetRandomLocation()
        {
            double x = randomSource.NextDouble();
            double y = randomSource.NextDouble();
            return new Location(x, y);
        }

        public Location[] GetLocationArray()
        {
            if (NumberOfNodes<2) { throw new ArgumentException("to few Nodes"); }            
            Location[] nodes = new Location[NumberOfNodes];
            for (int i = 0; i < NumberOfNodes; i++)
            {
                nodes[i] = GetRandomLocation();
            }
            return nodes;
        }

        public Connection[] GetConnectionArray(Location[] locations)
        {
            List<Connection> arcs = new List<Connection>();
            for (int i = 0; i < locations.Length-1; i++)
            {
                for (int j = i+1; j < locations.Length; j++)
                {
                    if (randomSource.NextDouble() > (1-Density))
                    {
                        var l1 = locations[i];
                        var l2 = locations[j];
                        arcs.Add(new Connection(l1, l2));
                        arcs.Add(new Connection(l2, l1));
                    }
                }
            }
            return arcs.ToArray();
        }

    }
}
