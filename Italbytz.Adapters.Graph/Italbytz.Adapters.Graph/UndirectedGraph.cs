using System;
using System.Collections.Generic;
using Italbytz.Ports.Graph;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;

namespace Italbytz.Adapters.Graph
{
    public class UndirectedGraph<TVertex, TEdge> : IUndirectedGraph<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        public UndirectedGraph()
        {
        }

        public IEnumerable<TEdge> Edges { get; set; }

        public string ToGraphviz()
        {
            if (typeof(TVertex) == typeof(string) && typeof(TEdge) == typeof(ITaggedEdge<string, double>))
            {
                var graph = ((IUndirectedGraph<string, ITaggedEdge<string, double>>)this).ToQuikGraph();
                return graph.ToGraphviz(algorithm =>
                {
                    algorithm.GraphFormat.RankDirection = GraphvizRankDirection.LR;
                    algorithm.FormatVertex += (sender, args) =>
                    {
                        args.VertexFormat.Label = $"{args.Vertex}";
                    };
                    algorithm.FormatEdge += (sender, args) =>
                    {
                        if (args.Edge is QuikGraph.TaggedEdge<string, double> edge)
                        {
                            args.EdgeFormat.Label.Value = $"{edge.Tag}";
                        }
                    };
                });
            }
            return "";
        }
    }
}
