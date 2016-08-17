using System.Web.Mvc;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public static class SiteNavViewModelExtensions
    {
        public static SiteNavViewModel SiteNav(this HtmlHelper html)
        {
            return DependencyResolver.Current.GetService<ISiteNavViewModelProvider>().Get();
        }
    }
}