using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.StartupTasks;

namespace Carsales.Web.Infrastructure.ViewEngines
{
    public class FeatureBasedRazorViewEngine : RazorViewEngine
    {
        private readonly string[] viewLocationFormats = {
            "~/Features/{1}/views/{0}.cshtml",
            "~/Features/Shared/{1}/Views/{0}.cshtml"
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