using System.Collections.Generic;
using System.Linq;

namespace ShortestPaths.Dijkstra
{
    public class ShortestPathTree
    {

        public ShortestPathTree(List<Arc> arcs, Node sourceNode)
        {
            Arcs = arcs;
            SourceNode = sourceNode;
        }

        public Node SourceNode { get; set; }

        public List<Arc> Arcs { get; set; }

        public bool IsEmpty { get { return (!Arcs.Any()); } }

        public double TotalWeight
        {
            get
            {
                return Arcs.Select(a => a.Weight).DefaultIfEmpty(0).Sum();
            }
        }

    }
}