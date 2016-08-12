using System.Web.Mvc;

namespace Carsales.Web.Features.Home
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}