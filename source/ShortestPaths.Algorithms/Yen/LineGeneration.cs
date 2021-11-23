using Microsoft.Extensions.Logging;
using ShortestPaths.Algorithms.Dijkstra;
using System;
using System.Collections.Generic;

namespace ShortestPaths.Algorithms.Yen
{
    public class LineGeneration
    {
        private readonly YenAlgorithm _algorithm;
        private readonly ILogger<LineGeneration> _logger;

        public LineGeneration(YenAlgorithm algorithm, ILogger<LineGeneration> logger)
        {
            _algorithm = algorithm;
            _logger = logger;
        }

        public List<ShortestPath> GenerateLines(double minLength, double maxLength, Graph graph)
        {
            _logger.LogTrace("Generating lines {0} - {1} with |A|={2}", minLength, maxLength,graph.Arcs.Length);
            List<ShortestPath> lines = new List<ShortestPath>();
            List<Tuple<Node, Node>> odPairs = GetOdPairs(graph);
            foreach (var pair in odPairs)
            {
                var topPaths = _algorithm.Find_k_ShortestPaths(graph, pair.Item1.Id, pair.Item2.Id, 100);
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
