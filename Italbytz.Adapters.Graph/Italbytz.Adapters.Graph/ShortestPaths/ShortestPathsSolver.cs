using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
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
    public class ShortestPathsSolver : AShortestPathsSolver
    {
        public ShortestPathsSolver() : base()
        {

        }

        public ShortestPathsSolver(string rootVertex) : base(rootVertex)
        {

        }

        protected override ShortestPathAlgorithmBase<string, QuikGraph.TaggedEdge<string, double>, IVertexListGraph<string, QuikGraph.TaggedEdge<string, double>>> GetAlgorithm(BidirectionalGraph<string, QuikGraph.TaggedEdge<string, double>> graph)
        {
            return new DijkstraShortestPathAlgorithm<string, QuikGraph.TaggedEdge<string, double>>(graph, ((edge) => edge.Tag));
        }

    }
}
