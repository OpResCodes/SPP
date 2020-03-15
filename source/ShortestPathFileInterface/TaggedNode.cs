using ShortestPaths.Dijkstra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShortestPathFileInterface
{
    [DebuggerDisplay("Node [{Id}], Name: {Name}")]
    public class TaggedNode : Node
    {
        public string Name { get; set; }
    }

}
