namespace TRuDI.Backend.Components
{
    using System;
    using System.Collections.Generic;

    using TRuDI.Models;
    using TRuDI.Models.BasicData;

    public class OriginalValueListRange
    {
        public OriginalValueListRange(DateTime start, DateTime end, OriginalValueList list, IEnumerable<IntervalReadingExt> items)
        {
            this.Source = list;
            this.Items = items;
            this.Start = start;
            this.End = end;
        }

        public DateTime Start { get; }

        public DateTime End { get; }

        public OriginalValueList Source { get; }

        public IEnumerable<IntervalReadingExt> Items { get; }
    }
}
