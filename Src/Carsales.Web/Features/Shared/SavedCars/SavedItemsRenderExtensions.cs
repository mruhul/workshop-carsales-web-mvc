using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Carsales.Web.Features.Shared.SavedCars
{
    public static class SavedCarsRenderExtensions
    {
        public static void RenderActionForSavedCars(this HtmlHelper html)
        {
            html.RenderAction("Index","SavedCars");
        }
    }
}