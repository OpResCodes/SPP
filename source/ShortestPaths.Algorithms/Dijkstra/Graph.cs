using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPaths.Algorithms.Dijkstra
{
    /// <summary>
    /// Represents the Network
    /// </summary>
    public class Graph
    {

        /// <summary>
        /// Create a new Network
        /// </summary>
        /// <param name="arcs">Set of directed connections</param>
        /// <param name="nodes">Set of Locations or Nodes</param>
        /// <remarks>
        /// Public Identifiers for Arcs and Nodes must be unique!
        /// </remarks>
        public Graph(Arc[] arcs, Node[] nodes)
        {
            if (arcs == null || !arcs.Any())
                throw new ArgumentException("No arcs specified.");
            if (nodes == null || !nodes.Any())
                throw new ArgumentException("No nodes specified.");

            Arcs = arcs;
            Nodes = nodes;
            NodeDictionary = new Dictionary<int, Node>(nodes.Length);
            for (int i = 0; i < Nodes.Length; i++)
            {
                NodeDictionary.Add(Nodes[i].Id, Nodes[i]);
            }
        }

        /// <summary>
        /// Set of connection arcs in the network
        /// </summary>
        public Arc[] Arcs { get; private set; }

        /// <summary>
        /// Set of Nodes in the Network
        /// </summary>
        public Node[] Nodes { get; private set; }

        /// <summary>
        /// Dictionary to performantly access Nodes by their Id
        /// </summary>
        public Dictionary<int, Node> NodeDictionary { get; private set; }

    }
}
