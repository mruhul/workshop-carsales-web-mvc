using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Configs;

namespace Carsales.Web.Features.Listings
{
    public class ListingSettings
    {
        public string BaseUrl { get; set; }
    }

    [AutoBind]
    public class ListingSettingsProvider : SettingsBase<ListingSettings>
    {
        protected override string SectionName => "ListingsSettings";
    }
}