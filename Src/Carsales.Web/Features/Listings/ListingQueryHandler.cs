using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Bolt.RequestBus;
using Bolt.RequestBus.Filters;
using Bolt.RequestBus.Handlers;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Extensions;
using Carsales.Web.Features.Shared.Proxies;
using Carsales.Web.Features.Shared.SavedCars;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Configs;

namespace Carsales.Web.Features.Listings
{
    public class ListingsRequest : IRequest
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }

    public class ListingViewModel
    {
        public long Total { get; set; }
        public IEnumerable<ListingItem> Items { get; set; }
    }

    public class ListingItem
    {
        public string NetworkId { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public string SellerType { get; set; }
        public string ListingType { get; set; }
        public string Odometer { get; set; }
        public string Transmission { get; set; }
        public string Body { get; set; }
        public string Price { get; set; }
        public bool IsSaved { get; set; }
    }

    [AutoBind]
    public class ListingQueryHandler : AsyncRequestHandlerBase<ListingsRequest,ListingViewModel>
    {
        private readonly IRequestBus bus;
        private readonly IRestClient restClient;
        private readonly ISettings<ProxyEndpointSettings> settings;

        public ListingQueryHandler(IRequestBus bus, IRestClient restClient, 
            ISettings<ProxyEndpointSettings> settings)
        {
            this.bus = bus;
            this.restClient = restClient;
            this.settings = settings;
        }

        protected override async Task<ListingViewModel> ProcessAsync(ListingsRequest msg)
        {
            var taskEvent = bus.PublishAsync(new ListingPageRequestedEvent());

            if (msg.Limit <= 0) msg.Limit = 15;

            var taskResponse = restClient.For(UrlBuilder
                .Host(settings.Value.Ryvuss)
                .Route("carlistingsubset")
                .QueryParam("q", $"((((Service=[Carsales]&(Make=[{msg.Make}]{(msg.Model.HasValue() ? "&Model=[{0}]".FormatWith(msg.Model) : string.Empty)})))))")
                .QueryParam("count", "true")
                .QueryParam("sr", $"latest||{msg.Offset}|{msg.Limit}", true))
                .Timeout(TimeSpan.FromSeconds(1))
                .RetryOnFailure(1)
                .GetAsync<RyvusSearchResponse>();

            await Task.WhenAll(taskEvent, taskResponse);

            var result = taskResponse.Result.Output;

            return result == null
                ? new ListingViewModel()
                : new ListingViewModel
                {
                    Total = result.Count,
                    Items = result.SearchResults
                            .NullSafe()
                            .Select(x => new ListingItem
                            {
                                Body = x.BodyStyleCategory,
                                NetworkId = x.Id,
                                Odometer = $"{x.Odometer:n0}",
                                Title = x.Title,
                                Photo = x.PhotoList?.FirstOrDefault(),
                                Transmission = x.Transmission,
                                Price = $"{x.Price:n0}",
                                SellerType = x.SellerType,
                                ListingType = x.ListingType
                            })
                };
        }
    }

    [AutoBind]
    public class PopulateSavedStatusFilter : AsyncRequestFilterBase<ListingsRequest, ListingViewModel>
    {
        private readonly ICurrentUserSavedCarsProvider provider;

        public PopulateSavedStatusFilter(ICurrentUserSavedCarsProvider provider)
        {
            this.provider = provider;
        }

        public override Task OnCompletedAsync(ListingsRequest request, ListingViewModel value)
        {
            var savedIds = provider.Get();

            value.Items = value.Items.Select(x =>
            {
                x.IsSaved = savedIds.Any(id => id == x.NetworkId);
                return x;
            });

            return Task.FromResult(0);
        }
    }
    

    public class RyvusSearchResponse
    {
        public IEnumerable<RyvusSearchItem> SearchResults { get; set; }
        public long Count { get; set; }
    }

    public class RyvusSearchItem
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string[] PhotoList { get; set; }
        public string SellerType { get; set; }
        public string ListingType { get; set; }
        public string Transmission { get; set; }
        public string BodyStyleCategory { get; set; }
        public string Odometer { get; set; }
        public string Price { get; set; }
        public string PriceType { get; set; }
    }
}