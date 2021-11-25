using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ShortestPaths.Algorithms.Dijkstra
{
    public sealed class Calculator
    {

        public Calculator(Graph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            Graph = graph;
        }

        /// <summary>
        /// Returns information about the last Computation
        /// </summary>
        public ComputationStats ComputationStats { get { return _stats; } }

        /// <summary>
        /// Graph must be set in the Constructor
        /// </summary>
        public Graph Graph { get; private set; }

        /// <summary>
        /// Calculates shortest paths from a source and returns all arcs of the shortest path tree
        /// </summary>
        /// <param name="sourceNodeId">Identifier of the source node</param>
        /// <param name="measureExecutionTime">if set to true, ComputationStats will include Algorithm time and data writing time</param>
        /// <returns></returns>
        public ShortestPathTree CalculateShortestPathTree(int sourceNodeId, bool measureExecutionTime = false)
        {
            //check node
            Node source = FindNodeInGraph(sourceNodeId);
            //calc if needed
            TryCalculateShortestPaths(source, measureExecutionTime);
            ShortestPathTree result;
            if (measureExecutionTime)
            {
                _timer.Restart();
                result = ConstructTree();
                _timer.Stop();
                _stats.WritingTimeInMilSec += _timer.ElapsedMilliseconds;
            }
            else
            {
                result = ConstructTree();
            }
            return result;
        }

        /// <summary>
        /// Calculates shortest paths from a source and returns one specific route between two nodes
        /// </summary>
        /// <param name="sourceNodeId">Identifier of the source node (origin)</param>
        /// <param name="sinkNodeId">Identifier of the sink node (destination)</param>
        /// <param name="measureExecutionTime">if set to true, ComputationStats will include Algorithm time and data writing time</param>
        /// <returns></returns>
        public ShortestPath CalculateShortestPath(int sourceNodeId, int sinkNodeId, bool measureExecutionTime = false)
        {
            //check nodes
            Node sink = FindNodeInGraph(sinkNodeId);
            Node source = FindNodeInGraph(sourceNodeId);
            //calc if needed
            TryCalculateShortestPaths(source, measureExecutionTime);
            //construct path
            ShortestPath result;
            if (measureExecutionTime)
            {
                _timer.Restart();
                result = ConstructPath(sinkNodeId);
                _timer.Stop();
                _stats.WritingTimeInMilSec += _timer.ElapsedMilliseconds;
            }
            else
            {
                result = ConstructPath(sinkNodeId);
            }
            return result;
        }

        public ShortestPath[] CalculateShortestPaths(int sourceNodeId, int[] sinkNodeIds, bool measureExecutionTime = false)
        {
            Node source = FindNodeInGraph(sourceNodeId);
            //calc if needed
            TryCalculateShortestPaths(source, measureExecutionTime);
            //construct results
            ShortestPath[] paths = new ShortestPath[sinkNodeIds.Length];
            if (measureExecutionTime)
            {
                _timer.Restart();
                for (int i = 0; i < sinkNodeIds.Length; i++)
                {
                    paths[i] = ConstructPath(sinkNodeIds[i]);
                }
                _timer.Stop();
                _stats.WritingTimeInMilSec += _timer.ElapsedMilliseconds;
            }
            else
            {
                for (int i = 0; i < sinkNodeIds.Length; i++)
                {
                    paths[i] = ConstructPath(sinkNodeIds[i]);
                }
            }
            return paths;
        }

        private Node SourceNode
        {
            get { return _sourceNode; }
            set
            {
                if (value == null)
                    throw new ArgumentException("NULL is invalid for SourceNode");
                if (value != _sourceNode)
                {
                    Reset();
                    _sourceNode = value;
                }
                else if (_stats.Status == CalculationStatus.Cancelled || _stats.Status == CalculationStatus.Error)
                {
                    Reset();
                }
            }
        }

        private void TryCalculateShortestPaths(Node source, bool withTimeMeasure)
        {
            SourceNode = source;
            if (_stats.Status == CalculationStatus.NotCalculated)
            {
                try
                {
                    _stats.NumberOfArcs = Graph.Arcs.Length;
                    _stats.NumberOfNodes = Graph.Nodes.Length;

                    if (withTimeMeasure)
                    {
                        _timer.Restart();
                        _dijkstra.Dijkstra(_sourceNode);
                        _timer.Stop();
                        _stats.ComputationTimeInMilSec = _timer.ElapsedMilliseconds;
                    }
                    else
                    {
                        _dijkstra.Dijkstra(_sourceNode);
                    }
                    _stats.Status = CalculationStatus.Finished;
                    _stats.StatusDetail = "Computation completed.";
                }
                catch (Exception ex)
                {
                    _stats.Status = CalculationStatus.Error;
                    _stats.StatusDetail = string.Format("Exception: {0}\nStackTrace:\n{1}", ex.Message, ex.StackTrace);
                    if (_timer.IsRunning)
                        _timer.Reset();
                }
            }
        }

        internal void Reset()
        {
            _dijkstra.Initialize(Graph);
            _stats.Reset();
            _timer.Reset();
        }

        private ShortestPath ConstructPath(int sinkId)
        {
            Node sink = FindNodeInGraph(sinkId);
            int sourceId = _sourceNode.Id;
            List<Arc> arcs = new List<Arc>();
            if (sink.IsConnectedToSource)
            {
                var n = sink;
                while (n.Id != sourceId)
                {
                    arcs.Add(n.OptimalPredecessorArc);
                    n = n.OptimalPredecessor;
                }
                arcs.Reverse();
            }
            return new ShortestPath(arcs);
        }

        private ShortestPathTree ConstructTree()
        {
            var arcs = new List<Arc>();
            foreach (var node in Graph.Nodes)
            {
                if (node.IsConnectedToSource && node.Id != _sourceNode.Id)
                {
                    arcs.Add(node.OptimalPredecessorArc);
                }
            }
            return new ShortestPathTree(arcs, _sourceNode);
        }

        private Node FindNodeInGraph(int id)
        {            
            if (Graph.NodeDictionary.TryGetValue(id, out Node? node))
                return node;
            else
                throw new ArgumentException("Node with Id " + id + "is not part of the Graph!");
        }

        //fields
        private Stopwatch _timer = new Stopwatch();
        private ComputationStats _stats = new ComputationStats();
        private Node _sourceNode = null;
        private readonly DijkstraCalculator _dijkstra = new DijkstraCalculator();
    }


    internal sealed class DijkstraCalculator
    {

        internal DijkstraCalculator()
        {
            Queue = new NodeQueue();
        }

        internal void Dijkstra(Node source)
        {
            Node u = source;
            //let source slide to first position
            Queue.UpdateNodePosition(source, 0);
            while (!Queue.IsEmpty)
            {
                u = Queue.ExtractMinimum();
                if (!u.IsConnectedToSource)
                {
                    break;
                }
                foreach (var arc in u.OutgoingConnections)
                {
                    var d = arc.Destination;
                    if (!d.IsProcessed)
                    {
                        Relax(u, d, arc);
                    }
                }
                u.IsProcessed = true;
            }
        }

        //public void Dijkstra(Node source, Node sink)
        //{
        //    Node u = source;
        //    //let source slide to first position
        //    Queue.UpdateNodePosition(source, 0);
        //    while (!Queue.IsEmpty)
        //    {
        //        u = Queue.ExtractMinimum();
        //        //if (u.DistanceFromSource == double.PositiveInfinity)
        //        //{
        //        //    break;
        //        //}
        //        if (u.Id == sink.Id)
        //        {
        //            u.IsProcessed = true;
        //            break;
        //        }
        //        foreach (var arc in u.OutgoingConnections)
        //        {
        //            Relax(arc);
        //            //if (!arc.Destination.IsProcessed)
        //            //{
        //            //    Relax(arc);
        //            //}
        //        }
        //        //u.IsProcessed = true;
        //    }
        //}

        private void Relax(Node o, Node d, Arc arc)
        {
            if (double.IsInfinity(arc.Weight))
                return;

            double stretch = o.DistanceFromSource + arc.Weight;
            if (d.DistanceFromSource > stretch)
            {
                Queue.UpdateNodePosition(d, stretch);
                d.OptimalPredecessorArc = arc;
            }
        }

        internal void Clear()
        {
            Queue.Clear();
        }

        internal void Initialize(Graph g)
        {
            Clear();
            for (int i = 0; i < g.Nodes.Length; i++)
            {
                g.Nodes[i].ResetCalculationData();
                Queue.Enqeue(g.Nodes[i]);
            }
        }

        internal NodeQueue Queue { get; private set; }
    }

}
