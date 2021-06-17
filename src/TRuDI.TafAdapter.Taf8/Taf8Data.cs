namespace TRuDI.TafAdapter.Taf8
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TRuDI.HanAdapter.Interface;
    using TRuDI.Models.CheckData;
    using TRuDI.TafAdapter.Interface;
    using TRuDI.TafAdapter.Interface.Taf8;

    public class Taf8Data : ITaf8Data
    {
        public Taf8Data()
        {
        }

        public TafId TafId => TafId.Taf8;

        public DateTime Begin { get; private set; }

        public DateTime End { get; private set; }

        public void SetBillingPeriod(DateTime begin, DateTime end)
        {
            this.Begin = begin;
            this.End = end;
        }

        public void SetRegisters(List<Taf8Register> registers)
        {
            this.Registers = registers;
        }

        public IReadOnlyList<Taf8Register> Registers { get; private set; }
    }
}
