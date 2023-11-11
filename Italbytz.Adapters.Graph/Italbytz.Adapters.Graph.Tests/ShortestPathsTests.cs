using Italbytz.Adapters.Graph;
using Italbytz.Ports.Graph;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Italbytz.Adapters.Graph.Tests
{
    public class ShortestPathsTests
    {
        IShortestPathsSolver solver;

        [SetUp]
        public void Setup()
        {
            solver = new ShortestPathsSolver();
        }

        [Test]
        public void TestSolverGivesSolution()
        {
            var parameters = new ShortestPathsParameters();
            var solution = solver.Solve(parameters);
            foreach (var path in solution.Paths)
            {
                System.Console.WriteLine(path.ToString());
            }
        }

        [Test]
        public void TestRomania()
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

            var vertices = new string[] { vertex0, vertex1, vertex2, vertex3, vertex4, vertex5, vertex6, vertex7, vertex8, vertex9, vertex10, vertex11, vertex12, vertex13, vertex14, vertex15, vertex16, vertex17, vertex18, vertex19 };
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
            var graph = new UndirectedGraph<string, ITaggedEdge<string, double>>() { Edges = edges };
            var parameters = new ShortestPathsParameters(vertices, graph);
            solver = new ShortestPathsSolver(vertex0);

            ((ShortestPathsSolver)solver).ManualDisjkstra(parameters);

            /*var solution = solver.Solve(parameters);
            foreach (var path in solution.Paths)
            {
                System.Console.WriteLine(path.ToString());
            }*/
        }

    }
}
