using System;
using System.Configuration;
using Autofac;
using Bolt.Cache;
using Bolt.Cache.Builders;
using Bolt.Cache.Impl;
using Bolt.CodeProfiler;
using Bolt.CodeProfiler.Builders;
using Bolt.Common.Extensions;
using Bolt.Logger;
using Bolt.Logger.NLog;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Dto;
using Bolt.Serializer;
using Bolt.Serializer.Json;
using Carsales.Web.Infrastructure.RestClientLog;
using Carsales.Web.Infrastructure.Stores;

namespace Carsales.Web.Ioc
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ContextStore<>)).As(typeof(IContextStore<>)).InstancePerRequest();
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

            builder.Register(x => CacheStoreBuilder.New().Build()).As<ICacheStore>().SingleInstance();

            builder.Register(x => CodeProfilerBuilder.New()
                .Logger(LoggerFactory.Create("CodeProfiler"))
                .Settings(new CodeProfilerSettings())
                .Build())
                .As<ICodeProfiler>()
                .SingleInstance();
        }
    }

    public class CodeProfilerSettings : ICodeProfilerSettings
    {
        public CodeProfilerSettings()
        {
            Enabled = ConfigurationManager.AppSettings["CodeProfiler.Enabled"].ToBoolean() ?? false;
        }
        public bool Enabled { get; }
    }
}