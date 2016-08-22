using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bolt.RequestBus;

namespace Carsales.Web.Features.Listings
{
    [RoutePrefix("listings")]
    public class ListingsController : Controller
    {
        private readonly IRequestBus bus;

        public ListingsController(IRequestBus bus)
        {
            this.bus = bus;
        }

        [HttpGet]
        [Route("{make}/{model?}", Name = "ListingsByMakeModel")]
        public async Task<ActionResult> Index(ListingsRequest request)
        {
            var vm = await bus.SendAsync<ListingsRequest, ListingViewModel>(request);

            return View(vm.Value);
        }
    }

    public static class ListingsUrlExtensions
    {
        public static string Listings(this UrlHelper url, string make)
        {
            return url.RouteUrl("ListingsByMakeModel", new {Make = make});
        }
    }
}