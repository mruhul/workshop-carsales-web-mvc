using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.StartupTasks;

namespace Carsales.Web
{
    [AutoBind]
    public class RouteSetupTask : IStartUpTask
    {
        public void Run()
        {
            var routes = RouteTable.Routes;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
