using ShortestPaths.Algorithms.Dijkstra;

namespace ShortestPaths.Algorithms.Yen
{
    public class DynamicArc : Arc
    {
        private readonly double originalWeight;

        public DynamicArc(int id, Node o, Node d, double weight) : base(id, o, d, weight)
        {
            originalWeight = weight;
        }

        public DynamicArc(Node o, Node d, double weight) : base(o, d, weight)
        {
            originalWeight = weight;
        }

        internal void UpdateWeight(double value = double.PositiveInfinity)
        {
            Weight = value;
        }

        internal void RestoreOriginalWeight()
        {
            Weight = originalWeight;
        }

        public new DynamicArc GenerateReverse()
        {
            var a = new DynamicArc(Destination, Origin, Weight);
            a.AutoAttach();
            return a;
        }

        public override string ToString()
        {
            return $"d({Origin.Id},{Destination.Id})={Weight.ToString()} | [{originalWeight.ToString()}]";
        }
    }

}
