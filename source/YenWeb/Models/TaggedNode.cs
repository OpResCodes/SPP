using ShortestPaths.Dijkstra;
using System.Diagnostics;

namespace YenWeb.Models
{
    [DebuggerDisplay("Node [{Id}], Name: {Name}")]
    public class TaggedNode : Node
    {
        public string Name { get; set; }
    }
}
