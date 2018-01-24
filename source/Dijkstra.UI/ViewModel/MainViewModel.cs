using System.Diagnostics;
using System.Threading.Tasks;
using Dijkstra.UI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using ShortestPaths;
using System.Linq;
using System.Collections.Generic;
using ShortestPaths.Dijkstra;
using System;

namespace Dijkstra.UI.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            MessengerInstance.Register<ClickedNotification>(this, LocationClicked);
        }

        private async void LocationClicked(ClickedNotification clickedLocation)
        {
            if (_Graph.Nodes.Any())
            {
                var clickedNode = NodeFinder.FindLocation(_Graph.Nodes, clickedLocation.RelativeX, clickedLocation.RelativeY);
                if (clickedNode != null)
                {
                    //await CalcShortestPaths(clickedNode.Id);

                    if (_node1Clicked)
                    {
                        _node2Id = clickedNode.Id;
                        await CalcShortestPaths(_node1Id, _node2Id);
                    }
                    else
                    {
                        _node1Id = clickedNode.Id;
                    }
                    _node1Clicked = !_node1Clicked;

                }
            }
        }

        private int _node1Id;
        private int _node2Id;
        private bool _node1Clicked = false;


        private async Task CalcShortestPaths(int nodeOneId,int nodeTwoId)
        {
            try
            {
                IsBusy = true;
                foreach (var item in _Graph.Arcs)
                {
                    if (item.IsEmphasized)
                    {
                        item.IsEmphasized = false;
                    }
                }
                Calculator calculator = new Calculator(_Graph.DijkstraGraph);
                var source = Graph.Nodes.Where(t => t.Id == nodeOneId).Single();

                var shortestPath = await Task.Run(() =>
                {
                    return calculator.CalculateShortestPath(nodeOneId, nodeTwoId, true);
                    //return calculator.CalculateShortestPathTree(sourceNodeId: nodeOneId, measureExecutionTime: true);
                });
                var selectedArcIds = shortestPath.OrderedArcs.Select(a => a.Id);
                var shortestArcs = _Graph.Arcs.Where(a => selectedArcIds.Contains(a.Id));
                Graph.ShortestPathConnections.Clear();
                foreach (Connection item in shortestArcs)
                {
                    item.IsEmphasized = true;
                    Graph.ShortestPathConnections.Add(item);
                }
                Graph.OnCalculationComplete();
                TotalCost = shortestPath.TotalWeight;                
                CalculationTime = calculator.ComputationStats.ComputationTimeInMilSec;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            { IsBusy = false; }
        }

        public RelayCommand<double> GenerateNetwork
        {
            get
            {
                return _GenerateNetwork
                    ?? (_GenerateNetwork = new RelayCommand<double>(
                                          async (density) => await OnGenerateNetwork(density),
                                          (i) => { return !IsBusy; }));
            }
        }

        private async Task OnGenerateNetwork(double density)
        {
            try
            {
                IsBusy = true;
                Graph.Clear();
                var nwg = new NetworkGenerator(NumOfNodes, density);
                Location[] lc = null;
                Connection[] cn = null;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ShowGraph = false;
                lc = await Task.Run(() => nwg.GetLocationArray());
                cn = await Task.Run(() => nwg.GetConnectionArray(lc));
                sw.Stop();
                Debug.WriteLine(string.Format("Network Generated: {0}", sw.ElapsedMilliseconds.ToString()));
                Debug.WriteLine($"Adding to UI Layer");
                sw.Reset();
                for (int i = 0; i < lc.Length; i++)
                {
                    Graph.Add(lc[i]);
                }
                for (int i = 0; i < cn.Length; i++)
                {
                    Graph.Add(cn[i]);
                }
                NumOfArcs = cn.Length;
                RaisePropertyChanged(() => Graph.Arcs);
                RaisePropertyChanged(() => Graph);
                sw.Stop();
                Debug.WriteLine($"Updated: {sw.ElapsedMilliseconds.ToString()} ms");
                ShowGraph = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public GraphViewModel Graph
        {
            get { return _Graph; }
            set { Set(() => Graph, ref _Graph, value); }
        }

        public double TotalCost
        {
            get { return _TotalCost; }
            set { Set(() => TotalCost, ref _TotalCost, value); }
        }

        public double CalculationTime
        {
            get { return _CalculationTime; }
            set { Set(() => CalculationTime, ref _CalculationTime, value); }
        }

        public int NumOfNodes
        {
            get { return _NumOfNodes; }
            set
            {
                if (value < 3)
                {
                    NumOfNodes = 3;
                }
                else
                {
                    Set(() => NumOfNodes, ref _NumOfNodes, value);
                }
            }
        }

        public bool IsBusy
        {
            get { return _IsBusy; }
            set { Set(() => IsBusy, ref _IsBusy, value); }
        }

        public int NumOfArcs
        {
            get { return _NumOfArcs; }
            set { Set(() => NumOfArcs, ref _NumOfArcs, value); }
        }

        public bool ShowGraph
        {
            get { return _ShowGraph; }
            set { Set(() => ShowGraph, ref _ShowGraph, value); }
        }

        private double _TotalCost = 0;
        private double _CalculationTime = 0;
        private int _NumOfNodes = 30;
        private bool _IsBusy = false;
        private int _NumOfArcs = 0;
        private bool _ShowGraph = false;
        private GraphViewModel _Graph = new GraphViewModel();
        private RelayCommand<double> _GenerateNetwork;
        private RelayCommand<int> _Calculate;
    }
}