using System.Web.Mvc;
using System.Web.Mvc.Html;
using Carsales.Web.Infrastructure.Stores;

namespace Carsales.Web.Features.Shared.SavedCars
{
    [RoutePrefix("saved-cars")]
    public class SavedCarsController : Controller
    {
        private readonly IContextStore<CurrentUserSavedCarIds> provider;

        public SavedCarsController(IContextStore<CurrentUserSavedCarIds> provider)
        {
            this.provider = provider;
        }

        [Route]
        // GET: SavedItems
        public ActionResult Index()
        {
            var vm = provider.Get().Value;
            return PartialView(vm);
        }
    }    
}