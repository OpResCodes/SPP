using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamsPaths
{
    public class GamsNode : ShortestPaths.Dijkstra.Node
    {

        public GamsNode(string label) : base()
        {
            GamsSetElement = label;
        }

        public string GamsSetElement { get; }

    }
}
