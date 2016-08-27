using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolt.CodeProfiler;
using Bolt.Common.Extensions;
using Bolt.Logger;
using Bolt.RequestBus;
using Bolt.RequestBus.Handlers;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Dto;
using Bolt.RestClient.Extensions;
using Carsales.Web.Features.Shared.Proxies;
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
        private readonly ICarSearchApiProxy carSearchApiProxy;
        private readonly ILogger logger;
        private readonly ICodeProfiler codeProfiler;

        public ListingQueryHandler(IRequestBus bus, 
            ICarSearchApiProxy carSearchApiProxy,
            ILogger logger, ICodeProfiler codeProfiler)
        {
            this.bus = bus;
            this.carSearchApiProxy = carSearchApiProxy;
            this.logger = logger;
            this.codeProfiler = codeProfiler;
        }

        protected override async Task<ListingViewModel> ProcessAsync(ListingsRequest msg)
        {
            using (codeProfiler.Start("ListingPage"))
            {
                var taskEvent = bus.PublishAsync(new ListingPageRequestedEvent());

                if (msg.Limit <= 0) msg.Limit = 15;

                var taskResponse = LoadFromApiAsync(msg);

                await Task.WhenAll(taskEvent, taskResponse);

                var result = taskResponse.Result.Output;

                return result == null
                    ? new ListingViewModel()
                    : new ListingViewModel
                    {
                        Total = result.Count,
                        Items = result.SearchResults
                            .NullSafe()
                            .Select(BuildListItem)
                    };
            }
        }

        private async Task<RestResponse<RyvusSearchResponse>> LoadFromApiAsync(ListingsRequest msg)
        {
            using (codeProfiler.Start("LoadSearchResultByMakes"))
            {
                return await carSearchApiProxy.GetAsync<RyvusSearchResponse>(new RyvusGetInput
                {
                    Count = "true",
                    Q =
                        $"((((Service=[Carsales]&(Make=[{msg.Make}]{(msg.Model.HasValue() ? "&Model=[{0}]".FormatWith(msg.Model) : string.Empty)})))))",
                    Sr = $"latest||{msg.Offset}|{msg.Limit}",

                });
            }
        }

        private static ListingItem BuildListItem(RyvusSearchItem x)
        {
            return new ListingItem
            {
                Body = x.BodyStyleCategory,
                NetworkId = x.Id,
                Odometer = x.Odometer?.ToString("N0"),
                Title = x.Title,
                Photo = x.PhotoList?.FirstOrDefault(),
                Transmission = x.Transmission,
                Price = x.Price?.ToString("N0"),
                SellerType = x.SellerType,
                ListingType = x.ListingType
            };
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
        public int? Odometer { get; set; }
        public decimal? Price { get; set; }
        public string PriceType { get; set; }
    }
}