using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Bolt.RequestBus;
using Bolt.RestClient;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public class LoadSiteNavOnPageLoad<TEvent> : IAsyncEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IRestClient restClient;

        public LoadSiteNavOnPageLoad(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task HandleAsync(TEvent eEvent)
        {
            throw new NotImplementedException();
        }
    }

    public interface IRequireSiteNav { }
}