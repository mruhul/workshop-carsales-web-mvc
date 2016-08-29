using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolt.CodeProfiler;
using Bolt.Logger;
using Bolt.RequestBus;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Extensions;
using Carsales.Web.Features.Shared.Proxies;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Configs;
using Carsales.Web.Infrastructure.Stores;
using Carsales.Web.Infrastructure.UserContext;

namespace Carsales.Web.Features.Shared.SavedCars
{
    public class LoadCurrentUserSavedCarsOnPageLoad<TEvent> : IAsyncEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IRestClient restClient;
        private readonly IContextStore<CurrentUserSavedCarIds> context;
        private readonly IUserContext userContext;
        private readonly ISettings<ProxyEndpointSettings> settings;
        private readonly ICodeProfiler codeProfiler;

        public LoadCurrentUserSavedCarsOnPageLoad(IRestClient restClient, 
            IContextStore<CurrentUserSavedCarIds> context,
            IUserContext userContext,
            ISettings<ProxyEndpointSettings> settings, ICodeProfiler codeProfiler)
        {
            this.restClient = restClient;
            this.context = context;
            this.userContext = userContext;
            this.settings = settings;
            this.codeProfiler = codeProfiler;
        }


        public async Task HandleAsync(TEvent eEvent)
        {
            if (!(eEvent is IRequireSavedItems)) return;
            using (codeProfiler.Start("LoadCurrentUserSavedIds"))
            {
                var response = await restClient.For(UrlBuilder
                .Host(settings.Value.SavedItems)
                .Route("members/{0}/saved-items", userContext.CurrentUserId)
                .QueryParam("IsConvertToShowRoom", false)
                .QueryParam("IncludeSellerDetails", false)
                .QueryParam("IncludeStockDetails", false)
                .QueryParam("IncludeTotalSaved", false))
                .Timeout(TimeSpan.FromSeconds(1))
                .RetryOnFailure(1)
                .GetAsync<SavedItemsApiResponse>();


                context.Set(new CurrentUserSavedCarIds
                {
                    Value = response.Output?.Items?.Select(x => x.AdDetails.NetworkId)
                });
            }
            
        }
    }
    

    public class SavedItemsApiResponse
    {
        public IEnumerable<SavedItemId> Items { get; set; }
    }

    public class SavedItemId
    {
        public AdDetails AdDetails { get; set; }
    }

    public class AdDetails
    {
        public string NetworkId { get; set; }
    }
}