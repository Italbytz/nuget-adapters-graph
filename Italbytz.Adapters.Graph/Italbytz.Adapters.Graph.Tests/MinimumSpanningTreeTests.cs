using NUnit.Framework;
using Italbytz.Ports.Graph;
using Italbytz.Adapters.Graph;

namespace Italbytz.Adapters.Graph.Tests
{
    public class MinimumSpanningTreeTests
    {
        IMinimumSpanningTreeSolver solver;

        [SetUp]
        public void Setup()
        {
            solver = new MinimumSpanningTreeSolver();
        }

        [Test]
        public void TestSolverGivesSolution()
        {
            var parameters = new MinimumSpanningTreeParameters();
            var solution = solver.Solve(parameters);
            foreach (var edge in solution.Edges)
            {
                System.Console.WriteLine(edge.ToString());
            }
        }

        [Test]
        public void TestToGeometryGraph()
        {
            var parameters = new MinimumSpanningTreeParameters();
            var graph = parameters.Graph;
            /*var geometryGraph = graph.ToGeometryGraph();
            Assert.NotNull(geometryGraph);*/
        }

        [Test]
        public void TestEdgeEquality()
        {
            var edge1 = new TaggedEdge<string, double>
            {
                Source = "A",
                Target = "B",
                Tag = 2
            };
            var edge2 = new TaggedEdge<string, double>
            {
                Source = "A",
                Target = "B",
                Tag = 2
            };
            Assert.AreEqual(edge1, edge2);
        }
    }
}
