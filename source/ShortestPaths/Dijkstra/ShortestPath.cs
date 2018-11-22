using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPaths.Dijkstra
{

    /// <summary>
    /// Represents a shortest path between two nodes
    /// </summary>
    public sealed class ShortestPath
    {
        public ShortestPath(List<Arc> arcs)
        {
            OrderedArcs = arcs;
            if(!IsEmpty)
            {
                TotalWeight = DestinationNode.DistanceFromSource;
            }
            else
            {
                TotalWeight = double.NaN;
            }
        }

        public List<Arc> OrderedArcs { get; set; }

        public List<Node> OrderedNodes
        {
            get
            {
                List<Node> nodes = new List<Node>();
                if (!IsEmpty)
                {
                    for (int i = 0; i < OrderedArcs.Count; i++)
                    {
                        nodes.Add(OrderedArcs[i].Origin);
                    }
                    nodes.Add(DestinationNode);
                }
                return nodes;
            }
        }

        public Node OriginNode
        {
            get
            {
                if (!IsEmpty)
                {
                    return OrderedArcs.First().Origin;
                }
                else
                {
                    return null;
                }
            }
        }

        public Node DestinationNode
        {
            get
            {
                if (!IsEmpty)
                {
                    return OrderedArcs.Last().Destination;
                }
                else
                {
                    return null;
                }
            }
        }

        public double TotalWeight { get; private set; }

        public bool IsEmpty
        {
            get { return !OrderedArcs.Any(); }
        }

        public override string ToString()
        {
            if (!IsEmpty)
            {
                StringBuilder sb = new StringBuilder();
                var n = OrderedNodes;
                int k = n.Count - 1;
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
