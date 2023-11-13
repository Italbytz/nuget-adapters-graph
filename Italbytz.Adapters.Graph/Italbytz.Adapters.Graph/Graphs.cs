using System;
using Italbytz.Ports.Graph;
using System.Collections.Generic;
using QuikGraph.Algorithms;

namespace Italbytz.Adapters.Graph
{
    public sealed class Graphs
    {
        private static readonly Lazy<Graphs> lazy =
            new(() => new Graphs());

        public static Graphs Instance { get { return lazy.Value; } }

        public UndirectedGraph<string, ITaggedEdge<string, double>> AIMARomania { get; }
        public Func<string, double> AIMARomaniaHeuristic { get; private set; }
        public UndirectedGraph<string, ITaggedEdge<string, double>> TanenbaumWetherall { get; }

        private Graphs()
        {
            AIMARomania = buildAIMARomania();
            TanenbaumWetherall = buildTanenbaumWetherall();
        }

        private UndirectedGraph<string, ITaggedEdge<string, double>>? buildTanenbaumWetherall()
        {
            var vertex0 = "A";
            var vertex1 = "B";
            var vertex2 = "C";
            var vertex3 = "D";
            var vertex4 = "E";
            var vertex5 = "F";
            var vertex6 = "G";
            var vertex7 = "H";

            var edges = new List<ITaggedEdge<string, double>>
            {
                new TaggedEdge<string, double>(vertex0, vertex1, 2.0),
                new TaggedEdge<string, double>(vertex0, vertex6, 6.0),
                new TaggedEdge<string, double>(vertex1, vertex2, 7.0),
                new TaggedEdge<string, double>(vertex1, vertex4, 2.0),
                new TaggedEdge<string, double>(vertex2, vertex3, 3.0),
                new TaggedEdge<string, double>(vertex2, vertex5, 3.0),
                new TaggedEdge<string, double>(vertex3, vertex7, 2.0),
                new TaggedEdge<string, double>(vertex4, vertex5, 2.0),
                new TaggedEdge<string, double>(vertex4, vertex6, 1.0),
                new TaggedEdge<string, double>(vertex5, vertex7, 2.0),
                new TaggedEdge<string, double>(vertex6, vertex7, 4.0)
            };

            return new UndirectedGraph<string, ITaggedEdge<string, double>>() { Edges = edges };
        }

        private UndirectedGraph<string, ITaggedEdge<string, double>> buildAIMARomania()
        {
            var vertex0 = "Arad";
            var vertex1 = "Timisoara";
            var vertex2 = "Zerind";
            var vertex3 = "Oradea";
            var vertex4 = "Lugoj";
            var vertex5 = "Mehadia";
            var vertex6 = "Drobeta";
            var vertex7 = "Sibiu";
            var vertex8 = "Rimnicu Vilcea";
            var vertex9 = "Craiova";
            var vertex10 = "Fagaras";
            var vertex11 = "Pitesti";
            var vertex12 = "Giurgiu";
            var vertex13 = "Bukarest";
            var vertex14 = "Urziceni";
            var vertex15 = "Neamt";
            var vertex16 = "Iasi";
            var vertex17 = "Vaslui";
            var vertex18 = "Hirsova";
            var vertex19 = "Eforie";

            var AIMARomaniaHeuristicDictionary = new Dictionary<string, double>
            {
                { vertex0, 366 },
                { vertex13, 0 },
                { vertex9,160 },
                { vertex6,242 },
                { vertex19,161 },
                { vertex10,176 },
                { vertex12,77 },
                { vertex18,151 },
                { vertex16,226 },
                { vertex4,244 },
                { vertex5,241 },
                { vertex15,234 },
                { vertex3,380 },
                { vertex11,100 },
                { vertex8,193 },
                { vertex7,253 },
                { vertex1,329 },
                { vertex14,80 },
                { vertex17,199 },
                { vertex2,374 }
            };
            AIMARomaniaHeuristic = AlgorithmExtensions.GetIndexer(AIMARomaniaHeuristicDictionary);

            var edges = new List<ITaggedEdge<string, double>>
            {
                new TaggedEdge<string, double>(vertex0, vertex1, 118.0),
                new TaggedEdge<string, double>(vertex0, vertex2, 75.0),
                new TaggedEdge<string, double>(vertex0, vertex7, 140.0),
                new TaggedEdge<string, double>(vertex1, vertex4, 111.0),
                new TaggedEdge<string, double>(vertex2, vertex3, 71.0),
                new TaggedEdge<string, double>(vertex3, vertex7, 151.0),
                new TaggedEdge<string, double>(vertex4, vertex5, 70.0),
                new TaggedEdge<string, double>(vertex5, vertex6, 75.0),
                new TaggedEdge<string, double>(vertex6, vertex9, 120.0),
                new TaggedEdge<string, double>(vertex7, vertex8, 80.0),
                new TaggedEdge<string, double>(vertex7, vertex10, 99.0),
                new TaggedEdge<string, double>(vertex8, vertex9, 146.0),
                new TaggedEdge<string, double>(vertex8, vertex11, 97.0),
                new TaggedEdge<string, double>(vertex9, vertex11, 138.0),
                new TaggedEdge<string, double>(vertex10, vertex13, 211.0),
                new TaggedEdge<string, double>(vertex11, vertex13, 101.0),
                new TaggedEdge<string, double>(vertex12, vertex13, 90.0),
                new TaggedEdge<string, double>(vertex13, vertex14, 85.0),
                new TaggedEdge<string, double>(vertex14, vertex17, 142.0),
                new TaggedEdge<string, double>(vertex14, vertex18, 98.0),
                new TaggedEdge<string, double>(vertex15, vertex16, 87.0),
                new TaggedEdge<string, double>(vertex16, vertex17, 92.0),
                new TaggedEdge<string, double>(vertex18, vertex19, 86.0)
            };
            return new UndirectedGraph<string, ITaggedEdge<string, double>>() { Edges = edges };
        }
    }
}

