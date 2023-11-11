using System;
using System.Collections.Generic;
using Italbytz.Ports.Graph;

namespace Italbytz.Adapters.Graph
{
    public class TaggedEdge<TVertex, TTag> : ITaggedEdge<TVertex, TTag>
    {
        public TaggedEdge()
        {
        }

        public TaggedEdge(TVertex source, TVertex target, TTag tag)
        {
            Source = source;
            Target = target;
            Tag = tag;
        }

        public TTag Tag { get; set; }

        public TVertex Source { get; set; }

        public TVertex Target { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TaggedEdge<TVertex, TTag> edge &&
                   EqualityComparer<TTag>.Default.Equals(Tag, edge.Tag) &&
                   EqualityComparer<TVertex>.Default.Equals(Source, edge.Source) &&
                   EqualityComparer<TVertex>.Default.Equals(Target, edge.Target);
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1} ({2})", Source, Target, Tag?.ToString() ?? "no tag");
        }
    }
}
