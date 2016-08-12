using System;
using Autofac;
using Carsales.Web.Infrastructure.Attributes;

namespace Carsales.Web.Ioc
{
    public class ConventionBasedModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new[] {typeof (Startup).Assembly};
            
            builder.RegisterAssemblyTypes(assemblies)
                .Where(a => a.IsClass &&
                            Attribute.IsDefined(a, typeof(AutoBindSingletonAttribute)))
                .AsImplementedInterfaces()
                .SingleInstance();


            builder.RegisterAssemblyTypes(assemblies)
                .Where(a => a.IsClass &&
                            Attribute.IsDefined(a, typeof(AutoBindAttribute)))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(a => a.IsClass &&
                            Attribute.IsDefined(a, typeof(AutoBindSelfAttribute)))
                .AsSelf();


            builder.RegisterAssemblyTypes(assemblies)
                .Where(a => a.IsClass &&
                            Attribute.IsDefined(a, typeof(AutoBindPerRequestAttribute)))
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }
    }
}