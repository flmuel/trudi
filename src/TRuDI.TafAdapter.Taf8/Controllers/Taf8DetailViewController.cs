namespace TRuDI.TafAdapter.Taf2.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using TRuDI.TafAdapter.Interface;
    using TRuDI.TafAdapter.Taf8.Components;

    public class Taf8DetailViewController : Controller
    {
        private readonly ITafData data;

        public Taf8DetailViewController(ITafData data)
        {
            this.data = data;
        }

        public ViewComponentResult SelectTariffViewDay(DateTime timestamp)
        {
            return this.ViewComponent(typeof(TariffDataView8), new { timestamp = timestamp.Date });
        }
    }
}
