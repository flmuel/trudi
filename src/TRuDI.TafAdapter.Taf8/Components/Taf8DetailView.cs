namespace TRuDI.TafAdapter.Taf8.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using TRuDI.TafAdapter.Interface;
    using TRuDI.TafAdapter.Interface.Taf8;

    public class Taf8DetailView : ViewComponent
    {
        private readonly ITaf8Data data;

        public Taf8DetailView(ITafData data)
        {
            this.data = data as ITaf8Data;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(this.View(this.data));
        }
    }
}
