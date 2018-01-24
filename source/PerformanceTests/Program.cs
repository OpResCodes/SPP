using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenerator;
using ShortestPaths.Dijkstra;
using System.Globalization;

namespace PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello.");
            //Console.WriteLine("Generating Data..");
            //TestClass tc = new TestClass();
            //Console.WriteLine("Done.");
            //Console.WriteLine("Testing Instance...");
            //for (int i = 0; i < 5; i++)
            //{
            //    tc.TestInstance(tc.BigInstance);
            //    Console.WriteLine("Test " + i + " completed.");
            //}
            //Console.WriteLine("Tests finished.");
            Console.WriteLine("Type 'calc' to define a graph or 'exit' to quit.");
            string i = Console.ReadLine();
            while (i.Equals("calc"))
            {
                Console.Write("Number of Nodes: ");
                int n = Convert.ToInt32((Console.ReadLine()));
                Console.Write("Density: ");
                double d = Convert.ToDouble(Console.ReadLine(), new CultureInfo("en-US"));
                Console.WriteLine("Generating Graph..");
                var dg = new DataGenerator.Dijkstra.GraphGenerator();
                var graph = dg.GenerateRandomGraph(n, d);
                Console.WriteLine("Calculating shortest path tree..");
                var calc = new Calculator(graph);
                calc.CalculateShortestPathTree(0, true);
                Console.WriteLine(calc.ComputationStats.ToString());
                Console.WriteLine("-----\nType 'calc' to define a graph or 'exit' to quit.");
                i = Console.ReadLine();
            }
            if (i.Equals("Quit"))
                Console.WriteLine("Goodbye.");


        }
    }
}
