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
    public class CarSearchInput
    {
        public string Q { get; set; }
        public string Sl { get; set; }
        public string Sr { get; set; }
        public string Count { get; set; }
    }

    public interface ICarSearchApiProxy
    {
        Task<RestResponse<T>> GetAsync<T>(CarSearchInput input);
    }

    [AutoBind]
    public class CarSearchApiProxy : ICarSearchApiProxy
    {
        private readonly IRestClient restClient;
        private readonly ISettings<ProxyEndpointSettings> settings;

        public CarSearchApiProxy(IRestClient restClient, ISettings<ProxyEndpointSettings> settings)
        {
            this.restClient = restClient;
            this.settings = settings;
        }

        public Task<RestResponse<T>> GetAsync<T>(CarSearchInput input)
        {
            var url = UrlBuilder.Host(settings.Value.CarSearch)
                .Route("carlistingsubset/carsales")
                .QueryParams(input);

            return restClient.For(url)
                .Timeout(TimeSpan.FromSeconds(1))
                .RetryOnFailure(1)
                .GetAsync<T>();
        }
    }
}