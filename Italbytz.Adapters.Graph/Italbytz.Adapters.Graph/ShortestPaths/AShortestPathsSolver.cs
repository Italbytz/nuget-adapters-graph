using System;
using System.Collections.Generic;
using Italbytz.Ports.Graph;
using QuikGraph;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;

namespace Italbytz.Adapters.Graph
{
    public abstract class AShortestPathsSolver : IShortestPathsSolver
    {
        protected string rootVertex;
        protected readonly Dictionary<string, double> verticesCost = new();
        protected readonly Dictionary<string, bool> expandedVertices = new();
        protected readonly Dictionary<(string, string, double), bool> examinedEdges = new();
        protected readonly Dictionary<(string, string, double), bool> treeEdges = new();

        protected Ports.Graph.IUndirectedGraph<string, ITaggedEdge<string, double>>? originalGraph;
        protected int file = 0;

        public AShortestPathsSolver() : this("A") { }

        public AShortestPathsSolver(string rootVertex)
        {
            this.rootVertex = rootVertex;
        }

        public IShortestPathsSolution Solve(IShortestPathsParameters parameters)
        {
            originalGraph = parameters.Graph;
            var graph = originalGraph.ToQuikGraph();

            var bidirectionalGraph = graph.ToBidirectionalGraph();
            var algorithm = GetAlgorithm(bidirectionalGraph);

            InitializeVerticesDictionaries(graph.Vertices);
            InitializeEdgesDictionaries(graph.Edges);
            SaveGraph();

            if (algorithm is DijkstraShortestPathAlgorithm<string, QuikGraph.TaggedEdge<string, double>>)
            {
                ((DijkstraShortestPathAlgorithm<string, QuikGraph.TaggedEdge<string, double>>)algorithm).ExamineEdge += new EdgeAction<string, QuikGraph.TaggedEdge<string, double>>(ExamineEdgeHandler);
            }

            algorithm.TreeEdge += new EdgeAction<string, QuikGraph.TaggedEdge<string, double>>(TreeEdgeHandler);
            var predecessorRecorder = new VertexPredecessorRecorderObserver<string, QuikGraph.TaggedEdge<string, double>>();
            using (predecessorRecorder.Attach(algorithm))
            {
                algorithm.Compute(rootVertex);
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

            return new ShortestPathsSolution
            {
                Paths = paths
            };
        }

        protected abstract ShortestPathAlgorithmBase<string, QuikGraph.TaggedEdge<string, double>, IVertexListGraph<string, QuikGraph.TaggedEdge<string, double>>> GetAlgorithm(BidirectionalGraph<string, QuikGraph.TaggedEdge<string, double>> graph);

        protected void SaveGraph()
        {
            ((UndirectedGraph<string, ITaggedEdge<string, double>>)originalGraph).ToGraphviz(false, expandedVertices, examinedEdges, treeEdges, $"dijkstra_{rootVertex}_{file}.dot");
            file++;
        }

        protected void TreeEdgeHandler(QuikGraph.TaggedEdge<string, double> edge)
        {
            treeEdges[(edge.Source, edge.Target, edge.Tag)] = true;
            SaveGraph();
            //Console.WriteLine($"Handling edge {edge.Source} -> {edge.Target}");
        }

        protected void ExamineEdgeHandler(QuikGraph.TaggedEdge<string, double> edge)
        {
            var pathCost = verticesCost[edge.Source] + edge.Tag;
            if (!expandedVertices[edge.Source])
            {
                expandedVertices[edge.Source] = true;
                //Console.WriteLine($"Expanding {edge.Source} with current cost {verticesCost[edge.Source]}");
                SaveGraph();
            }
            examinedEdges[(edge.Source, edge.Target, edge.Tag)] = true;
            SaveGraph();
            //Console.WriteLine($"Examining {edge.Source} -> {edge.Target}: {edge.Tag}");
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

        protected void InitializeVerticesDictionaries(IEnumerable<string> vertices)
        {
            foreach (var vertex in vertices)
            {
                expandedVertices[vertex] = false;
                verticesCost[vertex] = double.MaxValue;
            }
            verticesCost[rootVertex] = 0.0;
        }

        protected void InitializeEdgesDictionaries(IEnumerable<QuikGraph.TaggedEdge<string, double>> edges)
        {
            foreach (var edge in edges)
            {
                examinedEdges[(edge.Source, edge.Target, edge.Tag)] = false;
                treeEdges[(edge.Source, edge.Target, edge.Tag)] = false;
            }
        }

    }
}

