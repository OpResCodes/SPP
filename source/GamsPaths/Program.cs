using ShortestPaths.Dijkstra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GamsPaths
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string gdxFile = args[0].Substring(4);
            string v = args[1].Substring(2);
            string w = args[2].Substring(2);
            if (string.IsNullOrWhiteSpace(gdxFile))
            {
                Console.WriteLine("Supply gdx file name using gdx=");
            }
            if (string.IsNullOrWhiteSpace(v))
            {
                Console.WriteLine("Provide set of nodes using v=");
            }
            if (string.IsNullOrWhiteSpace("w"))
            {
                Console.WriteLine("Provide set of arcs using w=");
            }
            try
            {
                Console.WriteLine("GAMS Shortest Path Interface...");
                Console.WriteLine("Preparing data structure..");
                Graph graph = GenerateGraph(gdxFile, v, w);
                ShortestPaths.Dijkstra.Calculator c = new Calculator(graph);
                Console.WriteLine("Calculating...");
                int[] sinks = graph.Nodes.Select(n => n.Id).ToArray();
                long compTime = 0;
                List<ShortestPath> paths = new List<ShortestPath>(graph.Nodes.Length);
                for (int i = 0; i < graph.Nodes.Length; i++)
                {
                    paths.AddRange(c.CalculateShortestPaths(graph.Nodes[i].Id, sinks, true));
                    compTime += c.ComputationStats.ComputationTimeInMilSec;
                }
                Console.WriteLine("Writing results..");
                ReturnResultGdx(gdxFile, paths.ToArray(), compTime);
                Console.WriteLine($"Computation completed in {compTime} ms.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static void ReturnResultGdx(string gdxFile, ShortestPath[] paths, long compTime)
        {
            FileInfo f = new FileInfo(gdxFile);
            GAMS.GAMSWorkspace gamsProject = new GAMS.GAMSWorkspace(f.Directory.FullName);
            using (GAMS.GAMSDatabase db = gamsProject.AddDatabaseFromGDX(gdxFile))
            {
                GAMS.GAMSParameter resultParameter;
                try
                {
                    resultParameter = db.GetParameter("spp");
                    resultParameter.Clear();
                }
                catch
                {
                    resultParameter = db.AddParameter("spp", 2);
                }

                for (int i = 0; i < paths.Length; i++)
                {
                    if (!paths[i].IsEmpty)
                    {
                        resultParameter.AddRecord(
                            ((GamsNode)paths[i].OriginNode).GamsSetElement,
                            ((GamsNode)paths[i].DestinationNode).GamsSetElement).Value = paths[i].TotalWeight;

                    }
                }

                //add computation time
                GAMS.GAMSParameter statPar;
                try
                {
                    statPar = db.GetParameter("spp_stats");
                    statPar.Clear();
                }
                catch
                {
                    statPar = db.AddParameter("spp_stats", 1);
                }
                statPar.AddRecord("Time_seconds").Value = Convert.ToDouble(compTime) / 1000d;
                db.Export(f.FullName);
            }
        }

        private static Graph GenerateGraph(string gdxFile, string v, string w)
        {
            if (!File.Exists(gdxFile))
            {
                throw new ArgumentException("Specified gdx file not found.");
            }
            //Workspace
            FileInfo f = new FileInfo(gdxFile);
            GAMS.GAMSWorkspace gamsProject = new GAMS.GAMSWorkspace(f.Directory.FullName);

            using (GAMS.GAMSDatabase db = gamsProject.AddDatabaseFromGDX(gdxFile))
            {
                //read nodes
                Dictionary<string, GamsNode> nodeLookup = new Dictionary<string, GamsNode>();
                GAMS.GAMSSet gdxNodeSet = db.GetSet(v);
                foreach (GAMS.GAMSSetRecord record in gdxNodeSet)
                {
                    nodeLookup.Add(record.Keys[0], new GamsNode(record.Keys[0]));
                }
                //read arcs
                GAMS.GAMSParameter gdxWeightPar = db.GetParameter(w);
                Arc[] arcs = new Arc[gdxWeightPar.NumberRecords];
                int k = 0;
                foreach (GAMS.GAMSParameterRecord record in gdxWeightPar)
                {
                    string first = record.Keys[0];
                    string second = record.Keys[1];
                    arcs[k] = new Arc(nodeLookup[first], nodeLookup[second], record.Value == double.Epsilon ? 0 : record.Value);
                    k++;
                }
                return new Graph(arcs, nodeLookup.Values.ToArray());
            }
        }

    }
}
