using System;
using System.Collections.Generic;
using QuikGraph;
using QuikGraph.Algorithms;
using System.Linq;
using QuikGraph.Algorithms.MinimumSpanningTree;
using QuikGraph.Algorithms.Observers;
using Italbytz.Ports.Graph;

namespace Italbytz.Adapters.Graph
{
    public class MinimumSpanningTreeSolver : IMinimumSpanningTreeSolver
    {
        public MinimumSpanningTreeSolver()
        {
        }

        public IMinimumSpanningTreeSolution Solve(IMinimumSpanningTreeParameters parameters)
        {
            var graph = parameters.Graph.ToBasicGraphOnEdges();
            var mst = new MinimumSpanningTreeByPrim(graph, (edge) => ((WeightedEdge<double>)edge).Weight, graph.Edges.First().Source);
            var solution = mst.GetTreeEdges();

            return new MinimumSpanningTreeSolution
            {
                Edges = solution.Select(edge => ((WeightedEdge<double>)edge).ToGenericEdge())
            };
        }

    }

}
