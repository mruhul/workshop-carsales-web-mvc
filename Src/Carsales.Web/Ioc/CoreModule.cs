using System;
using Autofac;
using Bolt.Logger;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Dto;
using Bolt.Serializer.Json;
using Carsales.Web.Infrastructure.RestClientLog;

namespace Carsales.Web.Ioc
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => RestClientBuilder.New()
                .WithSerializer(new JsonSerializer())
                .WithLogger(Bolt.Logger.NLog.LoggerFactory.Create("Bolt.RestClient"))
                .WithTimeTakenNotifier(new LogBasedReportTimeTaken(Bolt.Logger.NLog.LoggerFactory.Create("Bolt.RestClient")))
                .Build()
            ).As<IRestClient>()
            .SingleInstance();

            builder.Register(x => Bolt.Logger.NLog.LoggerFactory.Create("Carsales.Web"))
                .As<Bolt.Logger.ILogger>() 
                .SingleInstance();

            // builder.RegisterGeneric(typeof(BookWorm.Web.Features.Shared.CategoryMenu.LoadCategoryMenuOnPageLoadEventHandler<>)).As(typeof(Bolt.RequestBus.IAsyncEventHandler<>));
            // builder.RegisterGeneric(typeof(BookWorm.Web.Features.Shared.LoginStatus.LoadLoginStatusOnPageLoadEventHandler<>)).As(typeof(Bolt.RequestBus.IAsyncEventHandler<>));
            // builder.RegisterGeneric(typeof(BookWorm.Web.Features.Shared.SavedBooks.LoadSavedBooksOnPageLoadEventHandler<>)).As(typeof(Bolt.RequestBus.IAsyncEventHandler<>));
            // builder.RegisterGeneric(typeof(BookWorm.Web.Features.Shared.Cart.LoadCartOnPageLoadEventHandler<>)).As(typeof(Bolt.RequestBus.IAsyncEventHandler<>));
        }
    }

    
}