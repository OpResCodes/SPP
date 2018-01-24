using GalaSoft.MvvmLight;
using ShortestPaths;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra.UI.ViewModel
{
    public class GraphViewModel : ObservableObject
    {
        
        public ShortestPaths.Dijkstra.Graph DijkstraGraph
        {
            get
            {
                return new ShortestPaths.Dijkstra.Graph(
                    _Arcs.Select(a => a.NetworkArc).ToArray(),
                    _Nodes.Select(n => n.NetworkNode).ToArray());
            }
        }

        public ObservableCollection<Connection> Arcs
        {
            get { return _Arcs; }
            set { Set(() => Arcs, ref _Arcs, value); }
        }
            
        public ObservableCollection<Location> Nodes
        {
            get { return _Nodes; }
            set { Set(() => Nodes, ref _Nodes, value); }
        }

        internal void OnCalculationComplete()
        {
            RaisePropertyChanged(() => ShortestPathConnections);
        }

        internal void Clear()
        {
            Nodes.Clear();
            Arcs.Clear();
            ShortestPathConnections.Clear();
        }

        internal void Add(Location node)
        {
            _Nodes.Add(node);
        }

        internal void Add(Connection arc)
        {
            _Arcs.Add(arc);
        }

        private ObservableCollection<Connection> _Arcs = new ObservableCollection<Connection>();
        private ObservableCollection<Location> _Nodes = new ObservableCollection<Location>();

        public List<Connection> ShortestPathConnections { get; set; } = new List<Connection>();
    }
}
