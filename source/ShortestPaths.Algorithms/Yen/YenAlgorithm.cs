﻿using Microsoft.Extensions.Logging;
using ShortestPaths.Algorithms.Dijkstra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPaths.Algorithms.Yen
{
    public class YenAlgorithm
    {

        ILogger<YenAlgorithm> _logger;

        public YenAlgorithm(ILogger<YenAlgorithm> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originId">Origin node</param>
        /// <param name="destinationId">Destination node</param>
        /// <param name="K">The number of k shortest paths</param>
        /// <returns></returns>
        public List<ShortestPath> Find_k_ShortestPaths(Graph graph, int originId, int destinationId, int K)
        {
            Calculator sppCalculator = new Calculator(graph);
            //get initial shortest path
            var shortestPath = sppCalculator.CalculateShortestPath(originId, destinationId);
            if (shortestPath.IsEmpty)
            {
                return new List<ShortestPath>();
            }

            List<ShortestPath> bestPaths = new List<ShortestPath>();
            List<ShortestPath> candidatePaths = new List<ShortestPath>();
            bestPaths.Add(shortestPath);
            _logger.LogTrace($"Initial shortest path: {shortestPath.ToString()}");
            //main iteration
            for (int k = 1; k < K; k++)
            {
                RestoreArcWeights();
                //iterate from origin to last node before destination (could be origin too)
                int q = bestPaths[k - 1].OrderedNodes.Length - 1;
                for (int i = 0; i < q; i++)
                {
                    _logger.LogTrace($"k={k}, i={i}, Spurnode={bestPaths[k - 1].OrderedNodes[i].Id}");
                    var lastRoot = GetRootArcsOfPath(bestPaths[k - 1], i);
                    for (int j = 0; j < k; j++)
                    {
                        var jRoot = GetRootArcsOfPath(bestPaths[j], i);
                        if (IsEqualRoot(lastRoot, jRoot))
                        {
                            //temporarily remove extension arc used in j
                            ModifyArcWeightAtPosition(bestPaths[j], i);
                        }
                    }
                    // exclude nodes already in the path
                    // by setting outgoing distances to inf (should not be part of solution anymore)
                    CancelRootNodes(bestPaths[k - 1], i);
                    //reset current shortest path values for recomputation
                    sppCalculator.Reset();
                    //compute a spur path
                    int spurNodeId = bestPaths[k - 1].OrderedNodes[i].Id;

                    ShortestPath spur = sppCalculator.CalculateShortestPath(spurNodeId, destinationId);
                    _logger.LogTrace($"k={k}; Computed spur: {spur.ToString()}");
                    //after computation - reset weights
                    RestoreArcWeights();
                    //combine and store path if available
                    if (!spur.IsEmpty)
                    {
                        //get the root of the old path
                        List<Arc> joinedListOfArcs = GetRootArcsOfPath(bestPaths[k - 1], i);

                        //join the newly computed sub path
                        joinedListOfArcs.AddRange(spur.OrderedArcs);
                        ShortestPath joinedPath = new ShortestPath(joinedListOfArcs);
                        joinedPath.TotalWeight = joinedPath.OrderedArcs.Sum(a => a.Weight);
                        //add the constructed path to the candidate list
                        candidatePaths.Add(joinedPath);
                        _logger.LogTrace($"Added candidate: {joinedPath.ToString()}");
                    }
                }
                //transfer the best path to A
                if (candidatePaths.Count > 0)
                {
                    candidatePaths.Sort((a, b) => a.TotalWeight.CompareTo(b.TotalWeight));
                    bestPaths.Add(candidatePaths[0]);
                    _logger.LogTrace($"Selected {candidatePaths[0].ToString()} for A");
                    candidatePaths.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
            RestoreArcWeights();
            return bestPaths;
        }

        private bool IsEqualRoot(List<Arc> lastRoot, List<Arc> jRoot)
        {
            if (jRoot.Count != lastRoot.Count)
            {
                return false;
            }

            for (int i = 0; i < lastRoot.Count; i++)
            {
                if (lastRoot[i].Id != jRoot[i].Id)
                {
                    return false;
                }
            }
            return true;
        }

        private void CancelRootNodes(ShortestPath path, int i)
        {
            //set all outgoing connections for the nodes in the root of 
            //the path to infinity so that these nodes wont be part of a shortest path anymore
            for (int j = 0; j < i; j++)
            {
                foreach (DynamicArc incidentArc in path.OrderedNodes[j].OutgoingConnections)
                {
                    DenyArc(incidentArc);
                }
            }
        }

        private List<Arc> GetRootArcsOfPath(ShortestPath path, int i)
        {
            List<Arc> arcs = new List<Arc>();
            if (i < 1 || path.OrderedNodes.Length - 1 < i)
            {
                return arcs;
            }

            int nodeId = path.OrderedNodes[i].Id;
            int j = -1;
            do
            {
                j++;
                arcs.Add(path.OrderedArcs[j]);
            }
            while (path.OrderedArcs[j].Destination.Id != nodeId);
            return arcs;
        }

        private void ModifyArcWeightAtPosition(ShortestPath p, int i)
        {
            Node origin = p.OrderedNodes[i];
            Node destination = p.OrderedNodes[i + 1];
            DynamicArc arc = FindArc(p.OrderedArcs, origin, destination);
            DenyArc(arc);
        }

        private DynamicArc FindArc(Arc[] arcArray, Node origin, Node destination)
        {
            for (int i = 0; i < arcArray.Length; i++)
            {
                if(arcArray[i].Origin.Equals(origin) && arcArray[i].Destination.Equals(destination))
                {
                    return (DynamicArc)arcArray[i];
                }
            }
            throw new InvalidOperationException("Arc not found.");
        }


        private void RestoreArcWeights()
        {
            foreach (DynamicArc arc in modifiedArcs)
            {
                arc.RestoreOriginalWeight();
            }
            modifiedArcs.Clear();
        }

        private void DenyArc(DynamicArc arc)
        {
            arc.UpdateWeight();
            if (!modifiedArcs.Contains(arc))
            {
                modifiedArcs.Add(arc);
            }
        }

        private List<DynamicArc> modifiedArcs = new List<DynamicArc>();

    }


}
