using System.Web.Mvc;
using Bolt.Common.Extensions;

namespace Carsales.Web.Infrastructure.AssetMappers
{
    public static class AssetMapExtensions
    {
        public static MvcHtmlString AssetCss(this HtmlHelper html, string name)
        {
            var url = DependencyResolver.Current.GetService<IAssetMapProvider>().Css(name);
            return url.HasValue()
                ? MvcHtmlString.Create($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{url}\"/>")
                : MvcHtmlString.Empty;
        }
        public static MvcHtmlString AssetJs(this HtmlHelper html, string name)
        {
            var url = DependencyResolver.Current.GetService<IAssetMapProvider>().Js(name);
            return url.HasValue()
                ? MvcHtmlString.Create($"<script src=\"{url}\"></script>")
                : MvcHtmlString.Empty;
        }
    }
}