using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Italbytz.Ports.Graph;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;

namespace Italbytz.Adapters.Graph
{
    public static class Extensions
    {
        public static BasicGraphOnEdges<Microsoft.Msagl.Core.GraphAlgorithms.IEdge> ToBasicGraphOnEdges(this Ports.Graph.IUndirectedGraph<string, ITaggedEdge<string, double>> graph)
        {
            var edges = graph.Edges.Select(edge => edge.ToWeightedEdge()).ToList();
            return new BasicGraphOnEdges<Microsoft.Msagl.Core.GraphAlgorithms.IEdge>(edges);
        }

        public static Ports.Graph.IUndirectedGraph<string, ITaggedEdge<string, double>> ToGenericGraph(this QuikGraph.UndirectedGraph<string, QuikGraph.TaggedEdge<string, double>> graph)
        {
            var edges = graph.Edges.Select(edge => edge.ToGenericEdge()).ToList();
            return new UndirectedGraph<string, ITaggedEdge<string, double>>() { Edges = edges };
        }

        public static QuikGraph.UndirectedGraph<string, QuikGraph.TaggedEdge<string, double>> ToQuikGraph(this Ports.Graph.IUndirectedGraph<string, ITaggedEdge<string, double>> graph)
        {
            var quikgraph = new QuikGraph.UndirectedGraph<string, QuikGraph.TaggedEdge<string, double>>();
            var edges = graph.Edges.Select(edge => edge.ToQuikEdge()).ToList();
            quikgraph.AddVerticesAndEdgeRange(edges);
            return quikgraph;
        }

        public static ITaggedEdge<string, double> ToGenericEdge(this WeightedEdge<double> edge)
            => new TaggedEdge<string, double>(((char)edge.Source).ToString(), ((char)edge.Target).ToString(), edge.Weight);

        public static ITaggedEdge<string, double> ToGenericEdge(this QuikGraph.TaggedEdge<string, double> edge)
            => new TaggedEdge<string, double>(edge.Source, edge.Target, edge.Tag);

        public static QuikGraph.TaggedEdge<string, double> ToQuikEdge(this ITaggedEdge<string, double> edge)
            => new QuikGraph.TaggedEdge<string, double>(edge.Source, edge.Target, edge.Tag);

        public static WeightedEdge<Double> ToWeightedEdge(this ITaggedEdge<string, double> edge) =>
        new WeightedEdge<double>()
        {
            Source = (int)edge.Source.ToCharArray()[0],
            Target = (int)edge.Target.ToCharArray()[0],
            Weight = edge.Tag
        };

    }
}
