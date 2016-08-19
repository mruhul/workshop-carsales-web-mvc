﻿using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Configs;

namespace Carsales.Web.Features.Shared.Proxies
{
    public class ProxyEndpointSettings
    {
        public string DataService { get; set; }
        public string Ryvuss { get; set; }
        public string SavedItems { get; set; }
        public string SiteNav { get; set; }
    }

    [AutoBind]
    public class ProxyEndpointSettingsProvider : SettingsBase<ProxyEndpointSettings>
    {
        protected override string SectionName => "ProxyEndpointSettings";
    }
}