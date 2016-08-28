using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Carsales.Web.Features.Details.RelatedArticles
{
    public static class RelatedArticlesRenderActionExtensions
    {
        public static void RenderActionForRelatedArticles(this HtmlHelper html)
        {
            html.RenderAction("Index","RelatedArticles");
        }
    }
}