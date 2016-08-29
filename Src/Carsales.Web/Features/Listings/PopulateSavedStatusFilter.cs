using System.Linq;
using System.Threading.Tasks;
using Bolt.RequestBus.Filters;
using Carsales.Web.Features.Shared.SavedCars;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Stores;

namespace Carsales.Web.Features.Listings
{
    [AutoBind]
    public class PopulateSavedStatusFilter : AsyncRequestFilterBase<ListingsRequest, ListingViewModel>
    {
        private readonly IContextStore<CurrentUserSavedCarIds> context;

        public PopulateSavedStatusFilter(IContextStore<CurrentUserSavedCarIds> context)
        {
            this.context = context;
        }

        public override Task OnCompletedAsync(ListingsRequest request, ListingViewModel value)
        {
            var savedIds = context.Get();

            value.Items = value.Items.Select(x =>
            {
                x.IsSaved = savedIds.Value?.Any(id => id == x.NetworkId) ?? false;
                return x;
            });

            return Task.FromResult(0);
        }
    }
}