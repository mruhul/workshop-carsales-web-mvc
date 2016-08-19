using Bolt.RequestBus;
using Carsales.Web.Features.Shared.SavedCars;
using Carsales.Web.Features.Shared.SiteNav;

namespace Carsales.Web.Features.Listings
{
    public class ListingPageRequestedEvent : IEvent, IRequireSiteNav, IRequireSavedItems
    {
    }
}