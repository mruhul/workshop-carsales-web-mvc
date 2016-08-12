using Autofac;

namespace Carsales.Web.Ioc
{
    public class GenericEventHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // builder.RegisterGeneric(typeof(LoadCategoryMenuOnPageLoadEventHandler<>)).As(typeof(IAsyncEventHandler<>));
        }
    }
}