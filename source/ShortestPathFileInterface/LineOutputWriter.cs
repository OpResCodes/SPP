using ShortestPathFileInterface;
using ShortestPaths.Dijkstra;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShortestPaths.Yen
{
    public class LineOutputWriter
    {
        public static void WriteArcSet(string filePath, Graph graph)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var a in graph.Arcs)
                {
                    writer.WriteLine($"{Name(a.Origin)}.{Name(a.Destination)}");
                }
            }
        }

        public static void WriteArcCosts(string filePath, Graph graph)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var a in graph.Arcs)
                {
                    writer.WriteLine($"{Name(a.Origin)}.{Name(a.Destination)} {a.Weight}");
                }
            }
        }

        public static void WriteLineSet(string filePath, List<ShortestPath> lines)
        {
            int k = 0;
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var line in lines)
                {
                    k++;
                    writer.WriteLine($"L{k}");
                }
            }
        }

        public static void WriteLineArcSet(string filePath, List<ShortestPath> lines)
        {
            int k = 0;
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var line in lines)
                {
                    k++;
                    foreach (var a in line.OrderedArcs)
                    {
                        writer.WriteLine($"L{k}.{Name(a.Origin)}.{Name(a.Destination)}");
                    }
                }
            }
        }

        public static string WriteText(List<ShortestPath> lines)
        {

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
                        a.Weight.ToString("N2"),
                        startToNodeWeight.ToString("N2"),
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
                        a.Weight.ToString("N2"),
                        startToNodeWeight.ToString("N2"),
                        Name(line.DestinationNode),
                        Name(line.OriginNode)
                        ));
                }
            }
            return writer.ToString();
        }

        public static void WriteCsv(string filePath, List<ShortestPath> lines)
        {
            int k = 0;
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Line;Typ;Node1;Node2;Weight;DistToNode2;LineOrigin;LineDestination");
                foreach (var line in lines)
                {
                    k++;
                    double startToNodeWeight = 0;
                    foreach (var a in line.OrderedArcs)
                    {
                        startToNodeWeight += a.Weight;
                        writer.WriteLine(string.Format("L{0};A;{1};{2};{3};{4};{5};{6}",
                            k.ToString(),
                            Name(a.Origin),
                            Name(a.Destination),
                            a.Weight.ToString("N2"),
                            startToNodeWeight.ToString("N2"),
                            Name(line.OriginNode),
                            Name(line.DestinationNode)
                            ));
                    }
                    startToNodeWeight = 0;
                    for (int i = line.OrderedArcs.Count - 1; i >= 0; i--)
                    {
                        var a = line.OrderedArcs[i];
                        startToNodeWeight += a.Weight;
                        writer.WriteLine(string.Format("L{0};B;{1};{2};{3};{4};{5};{6}",
                            k.ToString(),
                            Name(a.Destination),
                            Name(a.Origin),
                            a.Weight.ToString("N2"),
                            startToNodeWeight.ToString("N2"),
                            Name(line.DestinationNode),
                            Name(line.OriginNode)
                            ));
                    }
                }
            }
        }

        private static string Name(Node n)
        {
            return ((TaggedNode)n).Name;
        }
    }
}
