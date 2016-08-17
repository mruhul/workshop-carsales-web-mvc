using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Configs;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public class SiteNavSettings 
    {
        public string BaseUrl { get; set; }
    }

    [AutoBind]
    public class SettingsSection : SettingsBase<SiteNavSettings>
    {
        protected override string SectionName => "SiteNavSettings";
    }
}