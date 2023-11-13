using System;
using System.Collections.Generic;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ShortestPath;

namespace Italbytz.Adapters.Graph
{
    public class AStarShortestPathsSolver : AShortestPathsSolver
    {
        public AStarShortestPathsSolver() : base()
        {

        }

        public AStarShortestPathsSolver(string rootVertex) : base(rootVertex)
        {

        }

        protected override ShortestPathAlgorithmBase<string, QuikGraph.TaggedEdge<string, double>, IVertexListGraph<string, QuikGraph.TaggedEdge<string, double>>> GetAlgorithm(BidirectionalGraph<string, QuikGraph.TaggedEdge<string, double>> graph)
        {
            return new AStarShortestPathAlgorithm<string, QuikGraph.TaggedEdge<string, double>>(graph, ((edge) => edge.Tag), Graphs.Instance.AIMARomaniaHeuristic);
        }

    }
}
