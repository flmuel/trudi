namespace TRuDI.TafAdapter.Interface.Taf8
{
    using System;
    using System.Diagnostics;

    using TRuDI.Models;
    using TRuDI.Models.BasicData;

    [DebuggerDisplay("{ObisCode}, TariffId={TariffId}, Amount={Amount}")]
    public class Taf8Register
    {
        public Taf8Register()
        {
        }

        /// <summary>
        /// The reading type from the original value list used as source of this register.
        /// </summary>
        public ReadingType SourceType { get; set; }

        public ObisId ObisCode { get; set; }
    
        public long? Amount
        {
            get; set;
        }

        public DateTime Timestamp { get; set; }
    }
}
