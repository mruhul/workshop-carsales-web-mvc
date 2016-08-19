using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bolt.RequestBus;
using Bolt.RequestBus.Handlers;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Extensions;
using Bolt.RestClient.Impl;
using Carsales.Web.Features.Shared.Proxies;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Configs;

namespace Carsales.Web.Features.Shared.SavedCars.Api
{
    [RoutePrefix("api/saved-cars/list")]
    public class SavedCarListApiController : ApiController
    {
        private readonly IRequestBus bus;

        public SavedCarListApiController(IRequestBus bus)
        {
            this.bus = bus;
        }

        [Route]
        public async Task<IHttpActionResult> Get([FromUri]SavedCarListRequest request)
        {
            var vm = await bus.SendAsync<SavedCarListRequest, IEnumerable<SavedCarItem>>(request);
            return Json(vm);
        }
    }

    [AutoBind]
    public class SavedCarListRequestHandler : AsyncRequestHandlerBase<SavedCarListRequest, IEnumerable<SavedCarItem>>
    {
        private readonly IRestClient restClient;
        private readonly ISettings<ProxyEndpointSettings> settings;

        public SavedCarListRequestHandler(IRestClient restClient, ISettings<ProxyEndpointSettings> settings)
        {
            this.restClient = restClient;
            this.settings = settings;
        }

        protected override async Task<IEnumerable<SavedCarItem>> ProcessAsync(SavedCarListRequest msg)
        {
            var response = await 
                restClient.For(UrlBuilder.Host(settings.Value.DataService).Route("items").QueryParam("ids", msg.Ids))
                    .RetryOnFailure(1)
                    .Timeout(TimeSpan.FromSeconds(1))
                    .GetAsync<IEnumerable<dynamic>>();

            return response.Output?.Select(x => new SavedCarItem
            {
                NetworkId = x.Id,
                Title = x.Specification.Title,
                Photo = x.Media.Photos[0].PhotoPath,
                Price = x.Prices.Price
            });
        }
    }

    public class SavedCarListRequest : IRequest
    {
        public string Ids { get; set; }
    }

    public class SavedCarItem
    {
        public string NetworkId { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public int Price { get; set; }
    }
}