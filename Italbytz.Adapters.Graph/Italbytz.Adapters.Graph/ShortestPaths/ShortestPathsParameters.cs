using System;
using Italbytz.Extensions;
using QuikGraph;
using Italbytz.Ports.Graph;

namespace Italbytz.Adapters.Graph
{
    public class ShortestPathsParameters : IShortestPathsParameters
    {
        readonly Random _random = new Random();
        public Ports.Graph.IUndirectedGraph<string, ITaggedEdge<string, double>> Graph { get; set; }
        public String[] Vertices { get; set; }

        public ShortestPathsParameters()
        {
            Vertices = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };
            var graph = CreateRandomGraph();
            Graph = graph.ToGenericGraph();
        }

        private QuikGraph.UndirectedGraph<string, QuikGraph.TaggedEdge<string, double>> CreateRandomGraph()
        {
            var graph = new QuikGraph.UndirectedGraph<string, QuikGraph.TaggedEdge<string, double>>();
            var shuffledVertices = _random.ShuffledStrings(Vertices);
            var weights = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            for (int i = 0; i < shuffledVertices.Length - 1; i++)
            {
                graph.AddVerticesAndEdge(new QuikGraph.TaggedEdge<string, double>(shuffledVertices[i], shuffledVertices[i + 1], _random.RandomElement<double>(weights)));
            }

            var rnd = new Random();
            for (int i = 0; i < rnd.Next(5, 10); i++)
            {
                string vertex1, vertex2;
                do
                {
                    vertex1 = _random.RandomElement<string>(Vertices);
                    vertex2 = _random.RandomElement<string>(Vertices);
                } while (graph.ContainsEdge(vertex1, vertex2) || vertex1.Equals(vertex2));
                graph.AddVerticesAndEdge(new QuikGraph.TaggedEdge<string, double>(vertex1, vertex2, _random.RandomElement<double>(weights)));

            }

            return graph;
        }
    }
}
