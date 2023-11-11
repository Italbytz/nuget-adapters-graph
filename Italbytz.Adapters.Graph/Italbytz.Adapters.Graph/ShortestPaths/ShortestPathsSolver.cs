using System;
using System.Collections.Generic;
using Italbytz.Ports.Graph;
using Microsoft.Msagl.Core.ProjectionSolver;
using Microsoft.Msagl.GraphmapsWithMesh;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;
using QuikGraph.Collections;

namespace Italbytz.Adapters.Graph
{
    public class ShortestPathsSolver : IShortestPathsSolver
    {
        public String RootVertex { get; set; } = "A";

        public ShortestPathsSolver()
        {
        }

        public ShortestPathsSolver(string rootVertex)
        {
            RootVertex = rootVertex;
        }

        public void ManualDisjkstra(IShortestPathsParameters parameters)
        {
            var graph = parameters.Graph.ToQuikGraph();

            var bidirectionalGraph = graph.ToBidirectionalGraph();
            var dijkstra = new DijkstraShortestPathAlgorithm<string, QuikGraph.TaggedEdge<string, double>>(bidirectionalGraph, ((edge) => edge.Tag));
            // Attach a Vertex Predecessor Recorder Observer to give us the paths
            var predecessorRecorder = new VertexPredecessorRecorderObserver<string, QuikGraph.TaggedEdge<string, double>>();
            using (predecessorRecorder.Attach(dijkstra))
            {
                dijkstra.Compute(RootVertex);
            }

            var predecessors = predecessorRecorder.VerticesPredecessors;
            TryFunc<string, IEnumerable<QuikGraph.TaggedEdge<string, double>>>
                tryGetPaths = (string vertex, out IEnumerable<QuikGraph.TaggedEdge<string, double>> edges) => predecessors.TryGetPath(vertex, out edges);

            var paths = new List<string>();
            foreach (var vertex in parameters.Vertices)
            {
                if (vertex != RootVertex && tryGetPaths(vertex, out IEnumerable<QuikGraph.TaggedEdge<string, double>> path))
                {
                    var pathString = RootVertex;
                    var cost = 0;
                    var lastVertex = RootVertex;
                    foreach (var edge in path)
                    {
                        cost += (int)edge.Tag;
                        lastVertex = edge.GetOtherVertex(lastVertex);
                        pathString += $" -> {lastVertex}";
                    }
                    pathString += $" ({cost})";
                    paths.Add(pathString);
                }
            }

            foreach (var path in paths)
            {
                System.Console.WriteLine(path.ToString());
            }

            /*foreach (string vertex in graph.Vertices)
            {
                double distance = 0;
                var v = vertex;
                //Console.Write($"{RootVertex} -> ");
                while (predecessorRecorder.VerticesPredecessors.TryGetValue(v, out QuikGraph.TaggedEdge<string, double> predecessor))
                {
                    Console.Write($"{v} -> ");
                    distance += predecessor.Tag;
                    v = predecessor.Source;
                }
                Console.WriteLine($"{v}: {distance}");
            }*/
        }

        public IShortestPathsSolution Solve(IShortestPathsParameters parameters)
        {
            var graph = parameters.Graph.ToQuikGraph();


            TryFunc<string, IEnumerable<QuikGraph.TaggedEdge<string, double>>>
                tryGetPaths = graph.ShortestPathsDijkstra((edge) => edge.Tag, RootVertex);


            var paths = new List<string>();
            foreach (var vertex in parameters.Vertices)
            {
                if (vertex != RootVertex && tryGetPaths(vertex, out IEnumerable<QuikGraph.TaggedEdge<string, double>> path))
                {
                    var pathString = RootVertex;
                    var cost = 0;
                    var lastVertex = RootVertex;
                    foreach (var edge in path)
                    {
                        cost += (int)edge.Tag;
                        lastVertex = edge.GetOtherVertex(lastVertex);
                        pathString += $" -> {lastVertex}";
                    }
                    pathString += $" ({cost})";
                    paths.Add(pathString);
                }
            }
            return new ShortestPathsSolution
            {
                Paths = paths
            };
        }
    }
}
