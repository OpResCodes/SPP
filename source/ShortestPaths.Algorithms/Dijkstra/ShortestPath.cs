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
        public ShortestPath(List<Arc> arcs)
        {
            OrderedArcs = arcs;
            if (!IsEmpty)
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
                if (_orderedNodes == null)
                {
                    _orderedNodes = new List<Node>();
                    if (!IsEmpty)
                    {
                        for (int i = 0; i < OrderedArcs.Count; i++)
                        {
                            _orderedNodes.Add(OrderedArcs[i].Origin);
                        }
                        _orderedNodes.Add(DestinationNode);
                    }
                }
                return _orderedNodes;
            }
        }

        private List<Node> _orderedNodes = null;

        public Node OriginNode
        {
            get
            {
                if (!IsEmpty)
                {
                    return OrderedArcs[0].Origin;
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
                    return OrderedArcs[OrderedArcs.Count - 1].Destination;
                }
                else
                {
                    return null;
                }
            }
        }

        public double TotalWeight { get; set; }

        public bool IsEmpty
        {
            get { return OrderedArcs == null || OrderedArcs.Count == 0; }
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
