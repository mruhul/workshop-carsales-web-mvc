using Autofac;
using Bolt.RequestBus;
using Carsales.Web.Features.Shared.SiteNav;

namespace Carsales.Web.Ioc
{
    public class GenericEventHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(LoadSiteNavOnPageLoad<>)).As(typeof(IAsyncEventHandler<>));
        }
    }
}