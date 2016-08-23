using System.Threading.Tasks;
using System.Web.Mvc;
using Bolt.Cache;
using Bolt.Cache.Extensions;
using Bolt.RequestBus;
using Carsales.Web.Infrastructure.Cache;
using Carsales.Web.Infrastructure.Stores;
using Carsales.Web.Infrastructure.UserContext;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public class LoadSiteNavOnPageLoadEventHandler<TEvent> : IAsyncEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly ISiteNavApiProxy proxy;
        private readonly IContextStore<SiteNavViewModel> provider;
        private readonly ICacheStore cache;
        private readonly IUserContext userContext;
        private const string Key = "SiteNav";

        public LoadSiteNavOnPageLoadEventHandler(ISiteNavApiProxy proxy,
            IContextStore<SiteNavViewModel> provider,
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
            var vm = await cache.Profile(CacheProfile.Aggressive)
                .FetchAsync(LoadFromApi)
                .CacheIf(x => x.TopNavHtml != MvcHtmlString.Empty)
                .GetAsync(Key);
            
            provider.Set(vm);
        }

        private async Task<SiteNavViewModel> LoadFromApi()
        {
            var siteNavData = await proxy.GetAsync(userContext.CurrentUserId);

            return BuildViewModel(siteNavData);
        }

        private SiteNavViewModel BuildViewModel(SiteNavData data)
        {
            return data == null
                ? new SiteNavViewModel()
                : new SiteNavViewModel
                {
                    StyleTag = MvcHtmlString.Create(data.Style),
                    ScriptTag = MvcHtmlString.Create(data.Script),
                    FooterHtml = MvcHtmlString.Create(data.Footer),
                    TopNavHtml = MvcHtmlString.Create(data.TopNav)
                };
        }
    }
}