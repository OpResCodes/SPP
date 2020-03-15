using ShortestPaths.Dijkstra;
using System.Text;

namespace ShortestPathFileInterface
{
    public class PathPrinter
    {
        public static string PrintTaggedPath(ShortestPath p)
        {

            if (!p.IsEmpty)
            {
                StringBuilder sb = new StringBuilder();
                var n = p.OrderedNodes;
                int k = n.Count - 1;
                sb.AppendFormat("Path: {0} - {1}, Weight: {2}\n", ((TaggedNode)n[0]).Name, ((TaggedNode)n[k]).Name, p.TotalWeight.ToString("N2"));
                for (int i = 0; i < k; i++)
                {
                    sb.AppendFormat("[{0}]-", ((TaggedNode)n[i]).Name);
                }
                sb.AppendFormat("[{0}]", ((TaggedNode)n[k]).Name);
                return sb.ToString();
            }
            else
            {
                return "Path is empty.";
            }

        }
    }

}
