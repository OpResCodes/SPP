using ShortestPaths.Dijkstra;
using System;
using System.Collections.Generic;

namespace ShortestPaths.Yen
{
    public class LineGeneration
    {
        public List<ShortestPath> GenerateLines(double minLength, double maxLength, Graph graph)
        {
            List<ShortestPath> lines = new List<ShortestPath>();
            List<Tuple<Node, Node>> odPairs = GetOdPairs(graph);
            foreach (var pair in odPairs)
            {
                YenAlgorithm ya = new YenAlgorithm();
                var topPaths = ya.Find_k_ShortestPaths(graph, pair.Item1.Id, pair.Item2.Id, 100);
                foreach (var p in topPaths)
                {
                    if (p.TotalWeight >= minLength && p.TotalWeight <= maxLength)
                    {
                        lines.Add(p);
                    }
                }
            }
            return lines;
        }

        private List<Tuple<Node, Node>> GetOdPairs(Graph graph)
        {
            List<Tuple<Node, Node>> result = new List<Tuple<Node, Node>>();
            for (int i = 0; i < graph.Nodes.Length - 1; i++)
            {
                for (int j = i + 1; j < graph.Nodes.Length; j++)
                {
                    result.Add(Tuple.Create(graph.Nodes[i], graph.Nodes[j]));
                }
            }
            return result;
        }
    }


}
