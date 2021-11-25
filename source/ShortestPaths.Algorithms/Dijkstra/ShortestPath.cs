using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShortestPaths.Algorithms.Dijkstra
{

    /// <summary>
    /// Represents a shortest path between two nodes
    /// </summary>
    public sealed class ShortestPath
    {
        public ShortestPath(IEnumerable<Arc> arcs)
        {
            OrderedArcs = arcs.ToArray();
            if (!IsEmpty)
            {
                TotalWeight = DestinationNode.DistanceFromSource;

                OrderedNodes = new Node[OrderedArcs.Length + 1];
                for (int i = 0; i < OrderedArcs.Length; i++)
                {
                    OrderedNodes[i] = OrderedArcs[i].Origin;
                }
                OrderedNodes[^1] = DestinationNode;
            }
            else
            {
                TotalWeight = double.NaN;
                OrderedNodes = new Node[0];
            }            
        }

        private ShortestPath() : this(new Arc[0]) { }

        public Arc[] OrderedArcs { get; set; }

        public Node[] OrderedNodes { get; private set; }

        public Node OriginNode
        {
            get
            {
                return OrderedArcs[0].Origin;
            }
        }

        public Node DestinationNode
        {
            get
            {
                return OrderedArcs[^1].Destination;
            }
        }

        public double TotalWeight { get; set; }

        public bool IsEmpty
        {
            get { return OrderedArcs.Length == 0; }
        }

        public override string ToString()
        {
            if (!IsEmpty)
            {
                StringBuilder sb = new StringBuilder();
                var n = OrderedNodes;
                int k = n.Length - 1;
                sb.AppendFormat("Path: {0} - {1}, Weight: {2}\n", n[0].Id, n[k].Id, TotalWeight.ToString("N2"));
                for (int i = 0; i < k; i++)
                {
                    sb.AppendFormat("[{0}]-", n[i].Id);
                }
                sb.AppendFormat("[{0}]", n[k].Id);
                return sb.ToString();
            }
            else
            {
                return "Path is empty.";
            }

        }

    }
}
