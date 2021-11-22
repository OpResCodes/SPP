using ShortestPaths.Dijkstra;
using ShortestPaths.Yen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YenWeb.Models;

namespace YenWeb.Logic
{
    public class GraphConvert
    {
        public Graph ReadGraphFromString(string csvGraph, bool addMirrorArcs = false)
        {
            Dictionary<string, TaggedNode> readNodes = new Dictionary<string, TaggedNode>();
            Dictionary<string, DynamicArc> readArcs = new Dictionary<string, DynamicArc>();
            CultureInfo formatter = new CultureInfo("en-US");
            using (StringReader reader = new StringReader(csvGraph))
            {
                reader.ReadLine();
                while (reader.Peek() >= 0)
                {
                    string row = reader.ReadLine().Trim();
                    if (string.IsNullOrEmpty(row))
                    {
                        continue;
                    }

                    string[] col = row.Split(';');
                    string n1 = col[1];
                    string n2 = col[2];
                    double w = double.Parse(col[3].Replace(',', '.'), formatter);
                    string idx = $"{n1}_{n2}";
                    string idxReverse = $"{n2}_{n1}";
                    if (readArcs.ContainsKey(idx))
                    {
                        throw new System.Exception($"Duplicate arc: {idx}");
                    }

                    if (!readNodes.TryGetValue(n1, out TaggedNode node1))
                    {
                        node1 = new TaggedNode() { Name = n1 };
                        readNodes.Add(n1, node1);
                    }
                    if (!readNodes.TryGetValue(n2, out TaggedNode node2))
                    {
                        node2 = new TaggedNode() { Name = n2 };
                        readNodes.Add(n2, node2);
                    }
                    DynamicArc arc = new DynamicArc(node1, node2, w);
                    readArcs.Add(idx, arc);
                    if (addMirrorArcs && !readArcs.ContainsKey(idxReverse))
                    {
                        DynamicArc reverseArc = arc.GenerateReverse();
                        readArcs.Add(idxReverse, reverseArc);
                    }
                }
            }
            return new Graph(readArcs.Values.ToArray(), readNodes.Values.ToArray());
        }

        public string WriteText(List<ShortestPath> lines)
        {
            CultureInfo format = new CultureInfo("en-US");
            int k = 0;
            var writer = new StringBuilder();

            writer.AppendLine("Line;Typ;Node1;Node2;Weight;DistToNode2;LineOrigin;LineDestination");
            foreach (var line in lines)
            {
                k++;
                double startToNodeWeight = 0;
                foreach (var a in line.OrderedArcs)
                {
                    startToNodeWeight += a.Weight;
                    writer.AppendLine(string.Format("L{0};A;{1};{2};{3};{4};{5};{6}",
                        k.ToString(),
                        Name(a.Origin),
                        Name(a.Destination),
                        a.Weight.ToString("N4", format),
                        startToNodeWeight.ToString("N2", format),
                        Name(line.OriginNode),
                        Name(line.DestinationNode)
                        ));
                }
                startToNodeWeight = 0;
                for (int i = line.OrderedArcs.Count - 1; i >= 0; i--)
                {
                    var a = line.OrderedArcs[i];
                    startToNodeWeight += a.Weight;
                    writer.AppendLine(string.Format("L{0};B;{1};{2};{3};{4};{5};{6}",
                        k.ToString(),
                        Name(a.Destination),
                        Name(a.Origin),
                        a.Weight.ToString("N4", format),
                        startToNodeWeight.ToString("N4", format),
                        Name(line.DestinationNode),
                        Name(line.OriginNode)
                        ));
                }
            }
            return writer.ToString();
        }

        private string Name(Node n)
        {
            return ((TaggedNode)n).Name;
        }

    }
}
