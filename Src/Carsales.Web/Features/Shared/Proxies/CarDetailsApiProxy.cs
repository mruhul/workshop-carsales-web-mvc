using System;
using System.Threading.Tasks;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Dto;
using Bolt.RestClient.Extensions;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Configs;

namespace Carsales.Web.Features.Shared.Proxies
{
    public class CarDetailsInput
    {
        public string Ids { get; set; }
        public string Projection { get; set; }
    }

    public interface ICarDetailsApiProxy
    {
        Task<RestResponse<T>> GetAsync<T>(CarDetailsInput input);
    }

    [AutoBind]
    public class CarDetailsApiProxy : ICarDetailsApiProxy
    {
        private readonly IRestClient restClient;
        private readonly ISettings<ProxyEndpointSettings> settings;

        public CarDetailsApiProxy(IRestClient restClient, ISettings<ProxyEndpointSettings> settings)
        {
            this.restClient = restClient;
            this.settings = settings;
        }

        public Task<RestResponse<T>> GetAsync<T>(CarDetailsInput input)
        {
            var url = UrlBuilder.Host(settings.Value.CarDetails)
                .Route("items")
                .QueryParams(input);

            return restClient.For(url)
                .Timeout(TimeSpan.FromSeconds(1))
                .RetryOnFailure(1)
                .GetAsync<T>();
        }
    }
}