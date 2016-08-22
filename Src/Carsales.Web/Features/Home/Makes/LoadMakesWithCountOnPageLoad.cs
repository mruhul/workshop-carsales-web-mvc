using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolt.Cache;
using Bolt.Cache.Extensions;
using Bolt.Common.Extensions;
using Bolt.RequestBus;
using Carsales.Web.Features.Shared.Proxies;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Cache;
using Carsales.Web.Infrastructure.Stores;
using Expresso.Navigation;

namespace Carsales.Web.Features.Home.Makes
{
    [AutoBind]
    public class LoadMakesWithCountOnPageLoad : IAsyncEventHandler<HomePageRequestedEvent>
    {
        private readonly IRyvusApiProxy proxy;
        private readonly IContextStore<IEnumerable<MakesViewModelItem>> provider;
        private readonly ICacheStore cacheStore;
        private const string Key = "LoadMakesWithCountOnPageLoad:Count";

        public LoadMakesWithCountOnPageLoad(IRyvusApiProxy proxy,
            IContextStore<IEnumerable<MakesViewModelItem>> provider,
            ICacheStore cacheStore)
        {
            this.proxy = proxy;
            this.provider = provider;
            this.cacheStore = cacheStore;
        }

        public async Task HandleAsync(HomePageRequestedEvent eEvent)
        {
            var vm = await cacheStore.Profile(CacheProfile.Light)
                .FetchAsync(LoadFromProxy)
                .CacheIf(x => x.HasItem())
                .GetAsync(Key);

            provider.Set(vm);
        }

        private async Task<IEnumerable<MakesViewModelItem>> LoadFromProxy()
        {
            var response = await proxy.GetAsync<SelectListResponseDto>(new RyvusGetInput
            {
                Sl = "Make||Grouped",
                Count = "true"
            });

            return response.Output?.SelectListCollection.Elements.FirstOrDefault()?
                .ChildNodes
                .Select(x => new MakesViewModelItem
                {
                    Name = x.Name,
                    Count = x.Count
                })
                .OrderBy(x => x.Name);
        }
    }

    public class SelectListResponseDto
    {
        public int Count { get; set; }
        public SelectListCollection SelectListCollection { get; set; }
    }
}