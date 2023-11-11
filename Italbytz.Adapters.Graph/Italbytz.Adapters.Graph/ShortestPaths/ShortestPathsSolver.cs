using System;
using System.Collections.Generic;
using Italbytz.Ports.Graph;
using QuikGraph;
using QuikGraph.Algorithms;

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
