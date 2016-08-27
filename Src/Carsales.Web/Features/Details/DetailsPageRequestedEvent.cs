using Bolt.RequestBus;
using Carsales.Web.Features.Shared.SavedCars;
using Carsales.Web.Features.Shared.SiteNav;

namespace Carsales.Web.Features.Details
{
    public class DetailsPageRequestedEvent : IEvent, IRequireSiteNav, IRequireSavedItems
    {
        public string Id { get; set; }
    }
}