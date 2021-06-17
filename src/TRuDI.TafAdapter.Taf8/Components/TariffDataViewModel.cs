namespace TRuDI.TafAdapter.Taf8.Components
{
    using System;
    using TRuDI.TafAdapter.Interface.Taf8;

    public class TariffDataViewModel
    {
        public TariffDataViewModel(DateTime timestamp, ITaf8Data data)
        {
            this.Timestamp = timestamp;
            this.Data = data;
        }

        public DateTime Timestamp { get; }

        public ITaf8Data Data { get; }
    }
}