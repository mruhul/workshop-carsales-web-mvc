using System;
using Autofac;
using Bolt.Logger;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Dto;
using Bolt.Serializer;
using Bolt.Serializer.Json;
using Carsales.Web.Infrastructure.RestClientLog;

namespace Carsales.Web.Ioc
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonSerializer>().As<ISerializer>().SingleInstance();

            builder.Register(x => RestClientBuilder.New()
                .WithSerializer(x.Resolve<ISerializer>())
                .WithLogger(Bolt.Logger.NLog.LoggerFactory.Create("Bolt.RestClient"))
                .WithTimeTakenNotifier(new LogBasedReportTimeTaken(Bolt.Logger.NLog.LoggerFactory.Create("Bolt.RestClient")))
                .Build()
            ).As<IRestClient>()
            .SingleInstance();

            builder.Register(x => Bolt.Logger.NLog.LoggerFactory.Create("Carsales.Web"))
                .As<ILogger>() 
                .SingleInstance();
        }
    }

    
}