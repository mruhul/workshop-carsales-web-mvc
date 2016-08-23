using System.Web.Mvc;
using Carsales.Web.Infrastructure.Stores;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public static class SiteNavViewModelExtensions
    {
        public static SiteNavViewModel SiteNav(this HtmlHelper html)
        {
            return DependencyResolver.Current.GetService<IContextStore<SiteNavViewModel>>().Get();
        }
    }
}