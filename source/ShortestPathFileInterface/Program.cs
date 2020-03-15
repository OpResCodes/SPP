using ShortestPaths.Dijkstra;
using ShortestPaths.Yen;
using System;
using System.IO;
using System.Linq;

namespace ShortestPathFileInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            StartYenAlgorithm();

            //StartDijkstraAlgorithm();

        }

        private static void StartDijkstraAlgorithm()
        {
            Console.WriteLine("csv format: < string;node1;node2;weight >");
            string file = "";
            while (string.IsNullOrWhiteSpace(file) || !File.Exists(file))
            {
                Console.WriteLine("CSV Input filepath:");
                file = Console.ReadLine().Trim('"').Trim();
            }
            FileInfo fi = new FileInfo(file);
            Console.WriteLine($"Found file {fi.Name}.");
            Console.WriteLine("Mirror Arcs (make undirected?) Y/N?");
            string yn = Console.ReadLine().Trim();
            while (!(string.Equals(yn,"y", StringComparison.CurrentCultureIgnoreCase) | 
                string.Equals(yn,"n", StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine("Mirror Arcs (make undirected?) Y/N?");
                yn = Console.ReadLine().Trim();
            }
            bool mirror = yn.Equals("y", StringComparison.CurrentCultureIgnoreCase);
            CsvFileReader r = new CsvFileReader();
            ShortestPaths.Dijkstra.Calculator calculator = new Calculator(r.ReadGraphFromFile(file,mirror));
            Console.WriteLine("Choose start node");
            string node = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(node   ))
            {
                node = Console.ReadLine();
            }

            var o = calculator.Graph.Nodes.Where(n => ((TaggedNode)n).Name == node).FirstOrDefault();
            if(o == null)
            {
                Console.WriteLine("No such node exists...");
            }
            else
            {
                var tree = calculator.CalculateShortestPathTree(o.Id);
                Console.WriteLine("Calculated. Total Weight: " + tree.TotalWeight.ToString("N2"));
                string output = Path.Combine(fi.DirectoryName, fi.Name.Replace(fi.Extension, "_solution" + fi.Extension));
                CsvFileWriter.WriteArcs(output, tree.Arcs);
                Console.WriteLine($"Wrote results to: {output}");
            }
            Console.WriteLine("Hit Key to exit.");
            Console.ReadKey();
        }

        private static void StartYenAlgorithm()
        {
            Console.WriteLine("csv format: < string;node1;node2;weight >");
            string file = "";
            while (string.IsNullOrWhiteSpace(file) || !File.Exists(file))
            {
                Console.WriteLine("CSV Input filepath:");
                file = Console.ReadLine().Trim('"').Trim();
            }
            FileInfo fi = new FileInfo(file);
            Console.WriteLine($"Found file {fi.Name}.");
            double min; double max;
            string ml1 = "";
            while (!double.TryParse(ml1, out min))
            {
                Console.WriteLine("Minimum line length:");
                ml1 = Console.ReadLine();
            }
            string mxl = "";
            while (!double.TryParse(mxl, out max))
            {
                Console.WriteLine("Maximum line length: ");
                mxl = Console.ReadLine();
            }
            if (min > max)
            {
                Console.WriteLine("Minimum > Maximum. Quitting..");
            }
            else
            {
                Console.WriteLine("Hit key to start line generation.");
                Console.ReadKey();
                CsvFileReader r = new CsvFileReader();
                LineGeneration lg = new LineGeneration();
                try
                {
                    Graph graph = r.ReadGraphFromFile(file);
                    var paths = lg.GenerateLines(min, max, graph);
                    foreach (var p in paths)
                    {
                        Console.WriteLine(PathPrinter.PrintTaggedPath(p));
                    }
                    LineOutputWriter.WriteCsv(Path.Combine(fi.DirectoryName, fi.Name.Replace(fi.Extension, "_solution" + fi.Extension)), paths);
                    Console.WriteLine("wrote solution to folder " + fi.DirectoryName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Execution Error: {ex.Message}");
                }
                Console.WriteLine("Hit key to exit.");
                Console.ReadKey();
            }
        }
    }
}
