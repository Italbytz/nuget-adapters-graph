using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Italbytz.Ports.Graph;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
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
        private readonly string rootVertex;
        private Ports.Graph.IUndirectedGraph<string, ITaggedEdge<string, double>> originalGraph;
        int file = 0;
        private readonly Dictionary<string, double> verticesCost = new();
        private readonly Dictionary<string, bool> expandedVertices = new();
        private readonly Dictionary<(string, string, double), bool> examinedEdges = new();

        public ShortestPathsSolver() : this("A") { }

        public ShortestPathsSolver(string rootVertex)
        {
            this.rootVertex = rootVertex;
        }

        private void ExamineEdgeHandler(QuikGraph.TaggedEdge<string, double> edge)
        {
            examinedEdges[(edge.Source, edge.Target, edge.Tag)] = true;
            ((UndirectedGraph<string, ITaggedEdge<string, double>>)originalGraph).ToGraphviz(false, expandedVertices, examinedEdges, $"graph{file}.dot");
            file++;

            var pathCost = verticesCost[edge.Source] + edge.Tag;
            if (!expandedVertices[edge.Source])
            {
                expandedVertices[edge.Source] = true;
                Console.WriteLine($"Expanding {edge.Source} with current cost {verticesCost[edge.Source]}");
            }
            Console.WriteLine($"Examining {edge.Source} -> {edge.Target}: {edge.Tag}");
            if (verticesCost[edge.Target] == double.MaxValue)
            {
                verticesCost[edge.Target] = pathCost;
            }
            else
            {
                if (pathCost < verticesCost[edge.Target])
                {
                    verticesCost[edge.Target] = pathCost;
                }
            }
        }

        public IShortestPathsSolution Solve(IShortestPathsParameters parameters)
        {
            originalGraph = parameters.Graph;
            var graph = originalGraph.ToQuikGraph();

            var bidirectionalGraph = graph.ToBidirectionalGraph();
            var dijkstra = new DijkstraShortestPathAlgorithm<string, QuikGraph.TaggedEdge<string, double>>(bidirectionalGraph, ((edge) => edge.Tag));

            InitializeVerticesDictionaries(graph.Vertices);
            InitializeEdgesDictionaries(graph.Edges);
            ((UndirectedGraph<string, ITaggedEdge<string, double>>)originalGraph).ToGraphviz(false, expandedVertices, examinedEdges, $"graph{file}.dot");
            file++;

            dijkstra.ExamineEdge += new EdgeAction<string, QuikGraph.TaggedEdge<string, double>>(ExamineEdgeHandler);
            dijkstra.TreeEdge += new EdgeAction<string, QuikGraph.TaggedEdge<string, double>>(TreeEdgeHandler);
            dijkstra.DiscoverVertex += new VertexAction<string>(DiscoverVertexHandler);
            dijkstra.StartVertex += new VertexAction<string>(StartVertexHandler);
            dijkstra.InitializeVertex += new VertexAction<string>(InitializeVertexHandler);
            dijkstra.FinishVertex += new VertexAction<string>(FinishVertexHandler);
            var predecessorRecorder = new VertexPredecessorRecorderObserver<string, QuikGraph.TaggedEdge<string, double>>();
            using (predecessorRecorder.Attach(dijkstra))
            {
                dijkstra.Compute(rootVertex);
            }

            var predecessors = predecessorRecorder.VerticesPredecessors;
            TryFunc<string, IEnumerable<QuikGraph.TaggedEdge<string, double>>>
                tryGetPaths = (string vertex, out IEnumerable<QuikGraph.TaggedEdge<string, double>> edges) => predecessors.TryGetPath(vertex, out edges);

            var paths = new List<string>();
            foreach (var vertex in parameters.Vertices)
            {
                if (vertex != rootVertex && tryGetPaths(vertex, out IEnumerable<QuikGraph.TaggedEdge<string, double>> path))
                {
                    var pathString = rootVertex;
                    var cost = 0;
                    var lastVertex = rootVertex;
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

            ((UndirectedGraph<string, ITaggedEdge<string, double>>)originalGraph).ToGraphviz(false, expandedVertices, examinedEdges, $"graph{file}.dot");
            file++;



            return new ShortestPathsSolution
            {
                Paths = paths
            };
        }

        private void TreeEdgeHandler(QuikGraph.TaggedEdge<string, double> edge)
        {
            //Console.WriteLine($"Handling edge {edge.Source} -> {edge.Target}");
        }

        private void FinishVertexHandler(string vertex)
        {
            Console.WriteLine();
            //Console.WriteLine($"Finishing vertex {vertex}");
        }

        private void InitializeVertexHandler(string vertex)
        {
            //Console.WriteLine($"Initializing vertex {vertex}");
        }

        private void StartVertexHandler(string vertex)
        {
            //Console.WriteLine($"Starting vertex {vertex}");
        }

        private void DiscoverVertexHandler(string vertex)
        {
            //Console.WriteLine($"Discovering vertex {vertex}");            
        }

        private void InitializeVerticesDictionaries(IEnumerable<string> vertices)
        {
            foreach (var vertex in vertices)
            {
                expandedVertices[vertex] = false;
                verticesCost[vertex] = double.MaxValue;
            }
            verticesCost[rootVertex] = 0.0;
        }

        private void InitializeEdgesDictionaries(IEnumerable<QuikGraph.TaggedEdge<string, double>> edges)
        {
            foreach (var edge in edges)
            {
                examinedEdges[(edge.Source, edge.Target, edge.Tag)] = false;
            }
        }
    }
}
