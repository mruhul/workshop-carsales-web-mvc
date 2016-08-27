using System.Threading.Tasks;
using System.Web.Mvc;
using Bolt.RequestBus;
using Expresso.Abstraction;
using Microsoft.FSharp.Control;

namespace Carsales.Web.Features.Details
{
    [RoutePrefix("details")]
    public class DetailsController : Controller
    {
        private readonly IRequestBus bus;

        public DetailsController(IRequestBus bus)
        {
            this.bus = bus;
        }

        [Route("{title}/{id}")]
        public async Task<ActionResult> Index(DetailsQuery query)
        {
            var vm = await bus.SendAsync<DetailsQuery, DetailsViewModel>(query);

            if (vm.Value == null) return HttpNotFound();

            return View(vm.Value);
        }
    }

}