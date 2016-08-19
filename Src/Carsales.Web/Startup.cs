using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Bolt.Common.Extensions;
using Bolt.Logger;
using Carsales.Web.Infrastructure.StartupTasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Carsales.Web.Startup))]
namespace Carsales.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var config = new HttpConfiguration();

            builder.RegisterControllers(typeof(Startup).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyModules(typeof(Startup).Assembly,
                typeof(Bolt.RequestBus.Autofac.DependencyResolver).Assembly);
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            app.UseAutofacWebApi(config);
            app.UseWebApi(config);

            MapApiRoutes(config);

            RunStartupTasks(container);
        }

        private static void MapApiRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }

        private static void RunStartupTasks(IContainer container)
        {
            var startUpTasks = container.Resolve<IEnumerable<IStartUpTask>>();
            startUpTasks.ForEach(x =>
            {
                try
                {
                    x.Run();
                }
                catch (Exception e)
                {
                    container.Resolve<ILogger>().Error(e, e.Message);
                }
            });
        }
    }
}
