﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Carsales.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
        }
    }
}
