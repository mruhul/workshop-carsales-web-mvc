using System.Linq;
using System.Threading.Tasks;
using Bolt.RequestBus.Filters;
using Carsales.Web.Features.Shared.SavedCars;
using Carsales.Web.Infrastructure.Attributes;

namespace Carsales.Web.Features.Listings
{
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
}