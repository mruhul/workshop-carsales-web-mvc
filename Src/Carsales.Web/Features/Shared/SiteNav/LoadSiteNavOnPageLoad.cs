using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bolt.Cache;
using Bolt.Cache.Extensions;
using Bolt.RequestBus;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Extensions;
using Carsales.Web.Infrastructure.Cache;
using Carsales.Web.Infrastructure.Configs;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public class LoadSiteNavOnPageLoad<TEvent> : IAsyncEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IRestClient restClient;
        private readonly ISiteNavViewModelProvider provider;
        private readonly ICacheStore cache;
        private readonly ISettings<SiteNavSettings> settings;
        private const string Key = "SiteNav";

        public LoadSiteNavOnPageLoad(IRestClient restClient, 
            ISiteNavViewModelProvider provider,
            ICacheStore cache,
            ISettings<SiteNavSettings> settings)
        {
            this.restClient = restClient;
            this.provider = provider;
            this.cache = cache;
            this.settings = settings;
        }

        public async Task HandleAsync(TEvent eEvent)
        {
            if(!(eEvent is IRequireSiteNav)) return;
            var vm = await cache.Profile(CacheLife.Aggressive)
                .FetchAsync(LoadFromApi)
                .CacheIf(x => x.TopNavHtml != MvcHtmlString.Empty)
                .GetAsync(Key);
            
            provider.Set(vm);
        }

        private async Task<SiteNavViewModel> LoadFromApi()
        {
            var response = await restClient
                .For(UrlBuilder.Host(settings.Value.BaseUrl).Route("/navigation/carsales"))
                .Timeout(TimeSpan.FromSeconds(1))
                .RetryOnFailure(1)
                .GetAsync<SiteNavResponse>();

            var output = response?.Output;

            return output == null
                ? new SiteNavViewModel()
                : new SiteNavViewModel
                {
                    StyleTag = MvcHtmlString.Create(output.Data.Style),
                    ScriptTag = MvcHtmlString.Create(output.Data.Script),
                    FooterHtml = MvcHtmlString.Create(output.Data.Footer),
                    TopNavHtml = MvcHtmlString.Create(output.Data.TopNav)
                };
        }
    }

    internal class SiteNavResponse
    {
        public SiteNavData Data { get; set; }
    }

    internal class SiteNavData
    {
        public string Script { get; set; }
        public string Style { get; set; }
        public string TopNav { get; set; }
        public string Footer { get; set; }
    }
}