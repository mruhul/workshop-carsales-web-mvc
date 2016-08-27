using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Dto;
using Bolt.Serializer;
using Carsales.Web.Features.Shared.Proxies;
using Carsales.Web.Infrastructure.RestClientLog;
using Xunit;

namespace Carsales.Web.IntegrationTests
{
    public class Class1
    {

        [Fact]
        public void Test()
        {
            var fake = TestContainer.Fake();

            var d = fake.Setup(cfg =>
            {
                cfg.RegisterType<RequestInterceptor>().As<IRequestInterceptor>();

                cfg.Register(x => RestClientBuilder.New()
                .WithSerializer(x.Resolve<ISerializer>())
                .WithInterceptors(x.Resolve<IEnumerable<IRequestInterceptor>>())
                .WithLogger(Bolt.Logger.NLog.LoggerFactory.Create("Bolt.RestClient"))
                .WithTimeTakenNotifier(new LogBasedReportTimeTaken(Bolt.Logger.NLog.LoggerFactory.Create("Bolt.RestClient")))
                .Build()
            ).As<IRestClient>()
            .SingleInstance();
            }).Scope();

            var items = d.Resolve<IEnumerable<IRequestInterceptor>>();

            var api = d.Resolve<Carsales.Web.Features.Shared.Proxies.IRyvusApiProxy>();

            var response = api.GetAsync<dynamic>(new RyvusGetInput());

            d.Dispose();
        }
    }

    public class RequestInterceptor : IRequestInterceptor
    {
        public InterceptedResponse Intercept<TRestRequest>(TRestRequest request) where TRestRequest : RestRequest
        {
            throw new NotImplementedException();
        }

        public Task<InterceptedResponse> InterceptAsync<TRestRequest>(TRestRequest request) where TRestRequest : RestRequest
        {
            throw new NotImplementedException();
        }
    }
}
