using System;
using Italbytz.Extensions;
using QuikGraph;
using Italbytz.Ports.Graph;

namespace Italbytz.Adapters.Graph
{
    public class MinimumSpanningTreeParameters : IMinimumSpanningTreeParameters
    {
        readonly Random _random = new Random();

        public Ports.Graph.IUndirectedGraph<string, ITaggedEdge<string, double>> Graph { get; set; }

        public MinimumSpanningTreeParameters()
        {
            var graph = CreateRandomGraph();
            Graph = graph.ToGenericGraph();
        }

        private QuikGraph.UndirectedGraph<string, QuikGraph.TaggedEdge<string, double>> CreateRandomGraph()
        {
            var graph = new QuikGraph.UndirectedGraph<string, QuikGraph.TaggedEdge<string, double>>();
            var vertices = new string[] { "A", "B", "C", "D", "E", "F" };
            var shuffledVertices = _random.ShuffledStrings(vertices);
            var weights = new double[] { 2, 4, 19, 62, 100, 250 };
            for (int i = 0; i < shuffledVertices.Length - 1; i++)
            {
                graph.AddVerticesAndEdge(new QuikGraph.TaggedEdge<string, double>(shuffledVertices[i], shuffledVertices[i + 1], _random.RandomElement<double>(weights)));
            }

            var rnd = new Random();
            for (int i = 0; i < rnd.Next(3, 6); i++)
            {
                string vertex1, vertex2;
                do
                {
                    vertex1 = _random.RandomElement<string>(vertices);
                    vertex2 = _random.RandomElement<string>(vertices);
                } while (graph.ContainsEdge(vertex1, vertex2) || vertex1.Equals(vertex2));
                graph.AddVerticesAndEdge(new QuikGraph.TaggedEdge<string, double>(vertex1, vertex2, _random.RandomElement<double>(weights)));

            }
            return graph;
        }
    }
}
