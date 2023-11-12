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
            var graph = Graphs.Instance.AIMARomania;
            var vertices = graph.ToQuikGraph().Vertices.ToArray();
            var parameters = new ShortestPathsParameters(vertices, graph);
            solver = new ShortestPathsSolver("Arad");

            var solution = solver.Solve(parameters);
            foreach (var path in solution.Paths)
            {
                System.Console.WriteLine(path.ToString());
            }
        }

    }
}
