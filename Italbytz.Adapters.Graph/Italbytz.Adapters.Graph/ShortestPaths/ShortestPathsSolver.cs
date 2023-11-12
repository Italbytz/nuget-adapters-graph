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

        private void ExamineEdgeHandler(QuikGraph.TaggedEdge<string, double> edge)
        {
            Console.WriteLine($"Examining {edge.Source} -> {edge.Target}: {edge.Tag}");
        }

        public IShortestPathsSolution Solve(IShortestPathsParameters parameters)
        {
            var graph = parameters.Graph.ToQuikGraph();

            var bidirectionalGraph = graph.ToBidirectionalGraph();
            var dijkstra = new DijkstraShortestPathAlgorithm<string, QuikGraph.TaggedEdge<string, double>>(bidirectionalGraph, ((edge) => edge.Tag));
            dijkstra.ExamineEdge += new EdgeAction<string, QuikGraph.TaggedEdge<string, double>>(ExamineEdgeHandler);
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
            return new ShortestPathsSolution
            {
                Paths = paths
            };
        }
    }
}
