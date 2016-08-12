using System.Web.Mvc;

namespace Carsales.Web.Infrastructure.ViewEngines
{
    public class FeatureBasedRazorViewEngine : RazorViewEngine
    {
        private readonly string[] viewLocationFormats = {
            "~/Features/{1}/views/{0}.cshtml",
            "~/Features/Shared/{1}/Views/{0}.cshtml",
            "~/Features/Shared/Views/{0}.cshtml"
        };

        private readonly string[] areaLocationFormats = {
            "~/Features/{2}/{1}/views/{0}.cshtml",
            "~/Features/Shared/{2}/{1}/Views/{0}.cshtml"
        };

        private readonly string[] fileExtensions = { "cshtml" };

        public FeatureBasedRazorViewEngine()
        {
            FileExtensions = fileExtensions;
            ViewLocationFormats = viewLocationFormats;
            PartialViewLocationFormats = viewLocationFormats;
            MasterLocationFormats = viewLocationFormats;

            AreaViewLocationFormats = areaLocationFormats;
            AreaMasterLocationFormats = areaLocationFormats;
            AreaPartialViewLocationFormats = areaLocationFormats;
        }
    }
}