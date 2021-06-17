namespace TRuDI.TafAdapter.Taf8.Components
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using TRuDI.TafAdapter.Interface;
    using TRuDI.TafAdapter.Interface.Taf8;

    public class TariffDataView8 : ViewComponent
    {
        private readonly ITaf8Data data;

        public TariffDataView8(ITafData data)
        {
            this.data = data as ITaf8Data;
        }

        public IViewComponentResult Invoke(DateTime timestamp)
        {
            return this.View(new TariffDataViewModel(timestamp, this.data));
        }
    }
}
