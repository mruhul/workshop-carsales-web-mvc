using System;
using System.Threading.Tasks;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Extensions;
using Carsales.Web.Features.Shared.Proxies;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Configs;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public class SiteNavApiResponse
    {
        public SiteNavData Data { get; set; }
    }

    public class SiteNavData
    {
        public string Script { get; set; }
        public string Style { get; set; }
        public string TopNav { get; set; }
        public string Footer { get; set; }
    }

    public interface ISiteNavApiProxy
    {
        Task<SiteNavData> GetAsync(string memberId);
    }

    [AutoBind]
    public class SiteNavApiProxy : ISiteNavApiProxy
    {
        private readonly IRestClient restClient;
        private readonly ISettings<ProxyEndpointSettings> settings;

        public SiteNavApiProxy(IRestClient restClient, ISettings<ProxyEndpointSettings> settings)
        {
            this.restClient = restClient;
            this.settings = settings;
        }

        public async Task<SiteNavData> GetAsync(string memberId)
        {
            var response = await restClient
                .For(UrlBuilder
                    .Host(settings.Value.SiteNav)
                    .Route("/navigation/carsales")
                    .QueryParam("memberId", memberId))
                .Timeout(TimeSpan.FromSeconds(1))
                .RetryOnFailure(1)
                .GetAsync<SiteNavApiResponse>();

            return response?.Output?.Data;
        }
    }
}