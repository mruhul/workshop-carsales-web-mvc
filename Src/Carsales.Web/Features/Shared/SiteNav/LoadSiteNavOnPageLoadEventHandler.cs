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
using Carsales.Web.Infrastructure.UserContext;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public class LoadSiteNavOnPageLoadEventHandler<TEvent> : IAsyncEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IRestClient restClient;
        private readonly ISiteNavApiProxy proxy;
        private readonly ISiteNavViewModelProvider provider;
        private readonly ICacheStore cache;
        private readonly IUserContext userContext;
        private readonly ISettings<SiteNavSettings> settings;
        private const string Key = "SiteNav";

        public LoadSiteNavOnPageLoadEventHandler(ISiteNavApiProxy proxy, 
            ISiteNavViewModelProvider provider,
            ICacheStore cache,
            IUserContext userContext)
        {
            this.proxy = proxy;
            this.provider = provider;
            this.cache = cache;
            this.userContext = userContext;
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
            var siteNavData = await proxy.GetAsync(userContext.CurrentUserId);
            
            return siteNavData == null
                ? new SiteNavViewModel()
                : new SiteNavViewModel
                {
                    StyleTag = MvcHtmlString.Create(siteNavData.Style),
                    ScriptTag = MvcHtmlString.Create(siteNavData.Script),
                    FooterHtml = MvcHtmlString.Create(siteNavData.Footer),
                    TopNavHtml = MvcHtmlString.Create(siteNavData.TopNav)
                };
        }
    }

    
}