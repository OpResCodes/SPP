using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPaths.Algorithms.Dijkstra
{
    public sealed class OutputPrinter
    {
        /// <summary>
        /// Returns Path and length as string
        /// </summary>
        public static string PrintPath(ShortestPath path)
        {
            return path.ToString();
        }

        /// <summary>
        /// returns Computation Stats
        /// </summary>
        /// <param name="calculator"></param>
        /// <returns></returns>
        public static string PrintStats(ComputationStats stats)
        {
            StringBuilder csb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-US");
            string f = "Graph:\n  Nodes: {0}, Arcs: {1}\n";
            f = string.Format(f,
                stats.NumberOfNodes.ToString("N0", ci),
                stats.NumberOfArcs.ToString("N0", ci)
                );
            csb.Append(f);
            csb.AppendLine("Calculation:");
            csb.AppendFormat("  Status: {0}\n", stats.Status.ToString());
            csb.AppendFormat("  Status Detail: {0}\n", stats.StatusDetail);
            csb.AppendLine("Computationtimes (ms):");
            csb.AppendFormat("  Calculation:\t{0}\n", stats.ComputationTimeInMilSec.ToString("N0", ci));
            csb.AppendFormat("  Data-Writing:\t{0}\n", stats.WritingTimeInMilSec.ToString("N0", ci));
            return csb.ToString();
        }
    }
}
