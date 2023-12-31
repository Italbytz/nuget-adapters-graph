﻿using QuikGraph;
using QuikGraph.Algorithms.ShortestPath;

namespace Italbytz.Adapters.Graph
{
    public class ShortestPathsSolver : AShortestPathsSolver
    {
        public ShortestPathsSolver(string rootVertex = "A", bool saveGraphs = false) : base(rootVertex, saveGraphs)
        {

        }

        protected override ShortestPathAlgorithmBase<string, QuikGraph.TaggedEdge<string, double>, IVertexListGraph<string, QuikGraph.TaggedEdge<string, double>>> GetAlgorithm(BidirectionalGraph<string, QuikGraph.TaggedEdge<string, double>> graph)
        {
            return new DijkstraShortestPathAlgorithm<string, QuikGraph.TaggedEdge<string, double>>(graph, ((edge) => edge.Tag));
        }

    }
}
