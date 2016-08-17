using System.Web.Mvc;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public class SiteNavViewModel
    {
        public MvcHtmlString ScriptTag { get; set; }
        public MvcHtmlString StyleTag { get; set; }
        public MvcHtmlString FooterHtml { get; set; }
        public MvcHtmlString TopNavHtml { get; set; }
    }
}