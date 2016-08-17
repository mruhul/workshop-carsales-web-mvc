using System.Threading.Tasks;
using System.Web.Mvc;
using Bolt.RequestBus;

namespace Carsales.Web.Features.Home
{
    public class HomeController : Controller
    {
        private readonly IRequestBus bus;

        public HomeController(IRequestBus bus)
        {
            this.bus = bus;
        }

        // GET: Home
        public async Task<ActionResult> Index()
        {
            await bus.PublishAsync(new HomePageRequestedEvent());
            return View();
        }
    }
}