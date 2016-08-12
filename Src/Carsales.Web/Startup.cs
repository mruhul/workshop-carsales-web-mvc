using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
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
            
            builder.RegisterControllers(typeof(Startup).Assembly);

            builder.RegisterAssemblyModules(typeof(Startup).Assembly,
                typeof(Bolt.RequestBus.Autofac.DependencyResolver).Assembly);
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
            
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
