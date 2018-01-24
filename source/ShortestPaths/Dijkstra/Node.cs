using System;
using System.Collections.Generic;

namespace ShortestPaths.Dijkstra
{
    /// <summary>
    /// Represents a location or node in the network
    /// </summary>
    public class Node : IEquatable<Node>, IComparable<Node>
    {

        /// <summary>
        /// Create a new Node with a unique(!) identifier
        /// </summary>
        /// <param name="id">Unique(!) identifier</param>
        public Node(int id)
        {
            Id = id;
            OutgoingConnections = new HashSet<Arc>();
            ResetCalculationData();
        }

        /// <summary>
        /// Create a new Node without identifier, in this case the identifier is set automatically
        /// </summary>
        public Node() : this(NextId) { }

        /// <summary>
        /// Public Identifier
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Final shortest path distance
        /// </summary>
        public double DistanceFromSource { get; set; }
        
        /// <summary>
        /// Indicates wheter the node is accessible from the source node of the shortest path tree
        /// </summary>
        public bool IsConnectedToSource
        {
            get
            {
                return DistanceFromSource < double.PositiveInfinity;
            }
        }

        /// <summary>
        /// Origin of optimal predecessor arc (if none is available will return a self-reference)
        /// </summary>
        public Node OptimalPredecessor
        {
            get
            {
                if (OptimalPredecessorArc == null)
                    return this;
                else
                    return OptimalPredecessorArc.Origin;
            }
        }

        /// <summary>
        /// Final connection on the shortest path to this node
        /// </summary>
        public Arc OptimalPredecessorArc { get; internal set; }

        /// <summary>
        /// Adjacency list
        /// </summary>
        public HashSet<Arc> OutgoingConnections { get; private set; }

        /// <summary>
        /// Position in the NodeQueue of the Algorithm
        /// </summary>
        internal int PositionInQueue { get; set; }

        /// <summary>
        /// Algorithm flag
        /// </summary>
        internal bool IsProcessed { get; set; }

        /// <summary>
        /// Will reset the data needed for the algorithm
        /// </summary>
        internal void ResetCalculationData()
        {
            OptimalPredecessorArc = null;
            DistanceFromSource = double.PositiveInfinity;
            PositionInQueue = 0;
            IsProcessed = false;
        }
        
        /// <summary>
        /// For auto-setting of identifier
        /// </summary>
        private static int IdCounter = 0;

        /// <summary>
        /// For auto-setting of identifier
        /// </summary>
        private static int NextId { get { IdCounter++; return IdCounter; } }

        /// <summary>
        /// GetHashCode Method
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Equals method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var castedObj = obj as Node;
            if (castedObj == null)
                return false;
            return Equals(castedObj);
        }

        /// <summary>
        /// Equals method
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Node other)
        {
            if (other == null)
                return false;
            if (object.ReferenceEquals(this, other))
                return true;

            return Id == other.Id;
        }

        /// <summary>
        /// CompareTo method
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Node other)
        {
            return this.DistanceFromSource.CompareTo(other.DistanceFromSource);
        }


    }
}
