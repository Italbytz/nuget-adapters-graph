using System;
using Microsoft.Msagl.Core.GraphAlgorithms;

namespace Italbytz.Adapters.Graph
{
    public class WeightedEdge<TWeight> : BasicEdge
    {


        public TWeight? Weight { get; set; }
    }
}

