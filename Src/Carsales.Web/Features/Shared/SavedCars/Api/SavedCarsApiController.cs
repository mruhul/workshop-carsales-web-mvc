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
        private readonly ICarDetailsApiProxy carDetailsApi;

        public SavedCarListRequestHandler(ICarDetailsApiProxy carDetailsApi)
        {
            this.carDetailsApi = carDetailsApi;
        }

        protected override async Task<IEnumerable<SavedCarItem>> ProcessAsync(SavedCarListRequest msg)
        {
            var response = await carDetailsApi.GetAsync<IEnumerable<dynamic>>(new CarDetailsInput
            {
                Ids = msg.Ids
            });

            return response.Output?.Select(x => new SavedCarItem
            {
                NetworkId = x.Id,
                Title = x.Specification.Title,
                Photo = x.Media.Photos[0].PhotoPath,
                Price = x.Prices.Price.ToString("N0")
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
        public string Price { get; set; }
    }
}