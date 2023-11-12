using System;
using System.Collections.Generic;
using Italbytz.Ports.Graph;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;

namespace Italbytz.Adapters.Graph
{
    public class UndirectedGraph<TVertex, TEdge> : IUndirectedGraph<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        private bool darkMode = false;
        private Dictionary<string, bool>? markedVertices;
        private Dictionary<(string, string, double), bool>? markedEdges;
        private Dictionary<(string, string, double), bool>? boldEdges;

        public UndirectedGraph()
        {
        }

        public IEnumerable<TEdge> Edges { get; set; }

        public string ToGraphviz() => ToGraphviz(false, null, null, null, null);


        public string ToGraphviz(bool darkMode, Dictionary<string, bool>? markedVertices, Dictionary<(string, string, double), bool>? markedEdges, Dictionary<(string, string, double), bool>? boldEdges, string? fileName)
        {
            this.darkMode = darkMode;
            this.markedVertices = markedVertices;
            this.markedEdges = markedEdges;
            this.boldEdges = boldEdges;
            if (typeof(TVertex) == typeof(string) && typeof(TEdge) == typeof(ITaggedEdge<string, double>))
            {
                var graph = ((IUndirectedGraph<string, ITaggedEdge<string, double>>)this).ToQuikGraph();
                var graphViz = graph.ToGraphviz(StandardGraphvizAlgorithm);
                if (fileName != null)
                {
                    System.IO.File.WriteAllText(fileName, graphViz);
                }
                return graphViz;
            }
            return "";
        }

        private void StandardGraphvizAlgorithm(GraphvizAlgorithm<string, QuikGraph.TaggedEdge<string, double>> algorithm)
        {
            algorithm.GraphFormat.RankDirection = GraphvizRankDirection.LR;
            algorithm.GraphFormat.BackgroundColor = GraphvizColor.Transparent;
            algorithm.FormatVertex += (sender, args) =>
            {
                args.VertexFormat.Label = $"{args.Vertex}";
                args.VertexFormat.StrokeColor = darkMode ? GraphvizColor.White : GraphvizColor.Black;
                args.VertexFormat.FontColor = darkMode ? GraphvizColor.White : GraphvizColor.Black;
                if (markedVertices != null)
                {
                    args.VertexFormat.Style = markedVertices[args.Vertex] ? GraphvizVertexStyle.Solid : GraphvizVertexStyle.Dotted;
                }

            };
            algorithm.FormatEdge += (sender, args) =>
            {
                if (args.Edge is QuikGraph.TaggedEdge<string, double> edge)
                {
                    args.EdgeFormat.Label.Value = $"{edge.Tag}";
                    args.EdgeFormat.StrokeColor = darkMode ? GraphvizColor.White : GraphvizColor.Black;
                    args.EdgeFormat.FontColor = darkMode ? GraphvizColor.White : GraphvizColor.Black;
                    if (markedEdges != null && boldEdges != null)
                    {
                        args.EdgeFormat.Style = markedEdges[(args.Edge.Source, args.Edge.Target, args.Edge.Tag)] ?
                        (boldEdges[(args.Edge.Source, args.Edge.Target, args.Edge.Tag)] ?
                        GraphvizEdgeStyle.Bold : GraphvizEdgeStyle.Solid) : GraphvizEdgeStyle.Dotted;
                    }
                }
            };
        }
    }
}
