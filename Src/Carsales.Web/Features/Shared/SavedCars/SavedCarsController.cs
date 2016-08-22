using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Carsales.Web.Features.Shared.SavedCars
{
    [RoutePrefix("saved-cars")]
    public class SavedCarsController : Controller
    {
        private readonly ICurrentUserSavedCarsProvider provider;

        public SavedCarsController(ICurrentUserSavedCarsProvider provider)
        {
            this.provider = provider;
        }

        [Route]
        // GET: SavedItems
        public ActionResult Index()
        {
            var vm = provider.Get();
            return PartialView(vm);
        }
    }    
}