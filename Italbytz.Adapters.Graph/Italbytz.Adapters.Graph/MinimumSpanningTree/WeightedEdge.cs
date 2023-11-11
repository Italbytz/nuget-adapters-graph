using System;
using Microsoft.Msagl.Core.GraphAlgorithms;

namespace Italbytz.Adapters.Graph
{
    public class BasicEdge : IEdge
    {
        public BasicEdge()
        {
        }

        public int Source { get; set; }
        public int Target { get; set; }
    }
}

