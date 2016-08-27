using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Xunit;

namespace Carsales.Web.IntegrationTests
{
    
    public static class TestContainer
    {
        private static readonly IContainer Container;
        static TestContainer()
        {
            var builder = new ContainerBuilder();
            var config = new HttpConfiguration();

            builder.RegisterControllers(typeof(Startup).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyModules(typeof(Startup).Assembly,
                typeof(Bolt.RequestBus.Autofac.DependencyResolver).Assembly);

            Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);

        }

        public static IContainer Value => Container;

        public static TestFake Fake()
        {
            return new TestFake(Container);
        }
    }

    public class TestFake
    {
        private readonly IContainer container;
        private readonly List<Action<ContainerBuilder>> _modules;

        public TestFake(IContainer container)
        {
            this.container = container;
            _modules = new List<Action<ContainerBuilder>>();
        }

        public TestFake Setup(Action<ContainerBuilder> action)
        {
            _modules.Add(action);

            return this;
        }

        public ILifetimeScope Scope()
        {
            return container.BeginLifetimeScope(cf =>
            {
                foreach (var module in _modules)
                {
                    module.Invoke(cf);
                }
            });
        }
    }
}