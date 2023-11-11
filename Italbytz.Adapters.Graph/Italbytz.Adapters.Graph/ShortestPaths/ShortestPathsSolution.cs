using System;
using System.Collections.Generic;
using Italbytz.Ports.Graph;
using QuikGraph;

namespace Italbytz.Adapters.Graph
{
    public class ShortestPathsSolution : IShortestPathsSolution
    {
        public ShortestPathsSolution()
        {
        }

        public List<string> Paths { get; set; }
    }
}
