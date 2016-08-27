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
    public class EditorialSearchGetInput
    {
        public string Q { get; set; }
        public string Sl { get; set; }
        public string Sr { get; set; }
        public string Count { get; set; }
    }

    public interface IEditorialSearchApiProxy
    {
        Task<RestResponse<T>> GetAsync<T>(EditorialSearchGetInput input);
    }

    [AutoBind]
    public class EditorialSearchApiProxy : IEditorialSearchApiProxy
    {
        private readonly IRestClient restClient;
        private readonly ISettings<ProxyEndpointSettings> settings;

        public EditorialSearchApiProxy(IRestClient restClient, ISettings<ProxyEndpointSettings> settings)
        {
            this.restClient = restClient;
            this.settings = settings;
        }

        public Task<RestResponse<T>> GetAsync<T>(EditorialSearchGetInput input)
        {
            var url = UrlBuilder.Host(settings.Value.EditorialSearch)
                .Route("editoriallisting")
                .QueryParams(input);

            return restClient.For(url)
                .Timeout(TimeSpan.FromSeconds(1))
                .RetryOnFailure(1)
                .GetAsync<T>();
        }
    }
}