using ShortestPaths.Dijkstra;
using ShortestPaths.Yen;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShortestPathFileInterface
{
    public class CsvFileReader
    {
        //string;node1;node2;weight
        public Graph ReadGraphFromFile(string filePath, bool addMirrorArcs = false)
        {
            Dictionary<string, TaggedNode> readNodes = new Dictionary<string, TaggedNode>();
            Dictionary<string, DynamicArc> readArcs = new Dictionary<string, DynamicArc>();

            using (StreamReader reader = new StreamReader(filePath))
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
                    double w = double.Parse(col[3]);
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
                        DynamicArc reverseArc= arc.GenerateReverse();
                        readArcs.Add(idxReverse, reverseArc);
                    }
                }
            }
            return new Graph(readArcs.Values.ToArray(), readNodes.Values.ToArray());
        }
    }

    public class CsvFileWriter
    {

        public static void WriteArcs(string filePath, IEnumerable<Arc> arcs)
        {
            using (var w = new StreamWriter(filePath))
            {
                w.WriteLine("i;j;w(i,j)");
                foreach (var a in arcs)
                {
                    w.WriteLine(string.Format("{0};{1};{2}",
                        ((TaggedNode)a.Origin).Name,
                        ((TaggedNode)a.Destination).Name,
                        a.Weight.ToString("N2")));
                }
            }
        }

    }

}
