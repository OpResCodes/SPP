using ShortestPaths.Algorithms.Dijkstra;
using System.Diagnostics;

namespace PathGenerator.Web.Model
{
    [DebuggerDisplay("Node [{Id}], Name: {Name}")]
    public class TaggedNode : Node
    {
        public string Name { get; set; }
    }
}
