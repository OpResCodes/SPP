using Dijkstra.UI.ViewModel;
using ShortestPaths.Dijkstra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra.UI.Model
{
    internal class GraphGen
    {

        public Graph GenerateGraph(CellGridViewmodel vm)
        {
            ClearPath(vm);
            var activeCells = vm.Cells.Where(c => c.CellState == CellState.IsActive || c.CellState == CellState.IsSelected);
            Dictionary<int, Node> nodes = new Dictionary<int, Node>();
            List<Arc> arcs = new List<Arc>();
            foreach (var c in activeCells)
            {
                if (!nodes.ContainsKey(c.Id))
                    nodes.Add(c.Id, new Node(c.Id));

                foreach (var n in c.GetActiveNeighbours())
                {
                    if (!nodes.ContainsKey(n.Id))
                        nodes.Add(n.Id, new Node(n.Id));
                    Arc a = new Arc(nodes[c.Id], nodes[n.Id], 1.0);
                    arcs.Add(a);
                    nodes[c.Id].OutgoingConnections.Add(a);
                }
            }

            ShortestPaths.Dijkstra.Graph g = new ShortestPaths.Dijkstra.Graph(arcs.ToArray(), nodes.Select(n => n.Value).ToArray());
            return g;
        }

        private void ClearPath(CellGridViewmodel vm)
        {
            foreach (var cell in vm.Cells.Where(c => c.CellState == CellState.IsPath))
            {
                cell.CellState = CellState.IsActive;
            }
        }

        private void AddNode(Dictionary<int,Node> d, Node n)
        {
            if (!d.ContainsKey(n.Id))
            {
                d.Add(n.Id, n);
            }
        }

    }
}
