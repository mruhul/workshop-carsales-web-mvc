using Bolt.RequestBus;
using Carsales.Web.Features.Shared.SiteNav;

namespace Carsales.Web.Features.Home
{
    public class HomePageRequestedEvent : IEvent, IRequireSiteNav
    {
    }
}