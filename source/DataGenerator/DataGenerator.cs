using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShortestPaths.Dijkstra;

namespace DataGenerator.Dijkstra
{

    public class GraphGenerator
    {

        private Random randomSource = new Random();

        public Graph GenerateRandomGraph(int numberOfNodes, double density)
        {
            if (numberOfNodes < 3)
                throw new ArgumentException("Number of Nodes too low");
            if (density < 0.001 || density > 1)
                throw new ArgumentException("Density not between 0 and 1");

            Node[] nodes = new Node[numberOfNodes];
            for (int i = 0; i < numberOfNodes; i++)
            {
                nodes[i] = new Node(i);
            }

            for (int i = 0; i < numberOfNodes - 1; i++)
            {
                for (int j = i + 1; j < numberOfNodes; j++)
                {
                    if (randomSource.NextDouble() > (1 - density))
                    {
                        double w = randomSource.Next((j - i), 2 * (j - i));
                        nodes[i].OutgoingConnections.Add(new Arc(nodes[i], nodes[j], w));
                        nodes[j].OutgoingConnections.Add(new Arc(nodes[j], nodes[i], w));
                    }
                }
            }
            return new Graph(nodes.SelectMany(n => n.OutgoingConnections).ToArray(), nodes.ToArray());
        }
        
        public Tuple<Graph, Dictionary<int, string>> GenerateExampleGraph()
        {
            Dictionary<int, string> labels = new Dictionary<int, string>()
            {
                {0, "s" },
                {1, "t" },
                {2, "x" },
                {3, "y" },
                {4, "z" }
            };

            List<Node> sampleNodes = new List<Node>();
            for (int i = 0; i < 5; i++)
            {
                sampleNodes.Add(new Node(i));
            }
            List<Arc> sampleArcs = new List<Arc>()
            {
                new Arc(sampleNodes[0], sampleNodes[1], 10),
                new Arc(sampleNodes[0],sampleNodes[3],5),
                new Arc(sampleNodes[1],sampleNodes[3],2),
                new Arc(sampleNodes[1],sampleNodes[2],1),
                new Arc(sampleNodes[2],sampleNodes[4],4),
                new Arc(sampleNodes[3],sampleNodes[1],3),
                new Arc(sampleNodes[3],sampleNodes[2],9),
                new Arc(sampleNodes[3],sampleNodes[4],2),
                new Arc(sampleNodes[4],sampleNodes[0],7),
                new Arc(sampleNodes[4],sampleNodes[2],6)
            };
            sampleArcs.ForEach(a => a.Origin.OutgoingConnections.Add(a));
            Graph g = new Graph(sampleArcs.ToArray(), sampleNodes.ToArray());
            return Tuple.Create(g, labels);
        }
        
    }
}
