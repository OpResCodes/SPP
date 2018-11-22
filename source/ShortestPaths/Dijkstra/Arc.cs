using System;

namespace ShortestPaths.Dijkstra
{

    /// <summary>
    /// Represents a directed connection betweeen two nodes
    /// </summary>
    public class Arc : IEquatable<Arc>, IComparable<Arc>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="o">Origin Node</param>
        /// <param name="d">Destination Node</param>
        /// <param name="weight">Weight of Arc</param>
        public Arc(int id, Node o, Node d, double weight)
        {
            Id = id;
            Origin = o;
            Destination = d;
            Weight = weight;
            AutoAttach();
        }

        /// <summary>
        /// Create a new Connection without setting Id. Id is automatically set in this case
        /// </summary>
        /// <param name="o">Origin Node</param>
        /// <param name="d">Destination Node</param>
        /// <param name="weight">Weight of the Arc</param>
        public Arc(Node o, Node d, double weight) :this(GetNextId,o,d,weight) { }

        /// <summary>
        /// Public Identifier
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Origin Node of this Connection
        /// </summary>
        public Node Origin { get; private set; }

        /// <summary>
        /// Destination Node of this Connection
        /// </summary>
        public Node Destination { get; private set; }

        /// <summary>
        /// Weight of the Arc
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Adds the Arc to the Adjacency List of the Node if it is not yet included
        /// </summary>
        public void AutoAttach()
        {
            if (Origin.OutgoingConnections.Contains(this))
                return;
            Origin.OutgoingConnections.Add(this);
        }

        public Arc GenerateReverse()
        {
            var a = new Arc(this.Destination, this.Origin, this.Weight);
            a.AutoAttach();
            return a;
        }

        public bool Equals(Arc other)
        {
            if (other == null)
                return false;
            if (object.ReferenceEquals(this,other))
                return true;

            return Id == other.Id;

        }

        public int CompareTo(Arc other)
        {
            return this.Weight.CompareTo(other.Weight);
        }

        public override bool Equals(object obj)
        {
            var a = obj as Arc;
            return Equals(a);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// For Auto-Setting Ids
        /// </summary>
        private static int IdCounter = 0;

        /// <summary>
        /// For Auto-Setting Ids
        /// </summary>
        private static int GetNextId { get { IdCounter++; return IdCounter; } }

    }
}
