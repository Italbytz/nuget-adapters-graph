using System;
using System.Collections.Generic;
using Microsoft.Msagl.Core.GraphAlgorithms;

namespace Italbytz.Adapters.Graph
{
    public class MSTExample
    {
        public MSTExample()
        {
        }

        public static void Example()
        {
            var edges = new List<BasicEdge>();
            edges.Add(new BasicEdge() { Source = 0, Target = 1 });
            edges.Add(new BasicEdge() { Source = 1, Target = 2 });
            edges.Add(new BasicEdge() { Source = 0, Target = 3 });
            edges.Add(new BasicEdge() { Source = 2, Target = 3 });

            var graph = new BasicGraphOnEdges<IEdge>(edges, 6);
            var mst = new MinimumSpanningTreeByPrim(graph, (edge) => 2.0, 0);
            var solution = mst.GetTreeEdges();
        }
    }
}

