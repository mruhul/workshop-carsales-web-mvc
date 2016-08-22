using System;
using Carsales.Web.Infrastructure.Stores;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Bolt.Common.Extensions;
using Bolt.RestClient;
using Bolt.RestClient.Builders;
using Bolt.RestClient.Extensions;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Configs;
using Expresso.Navigation;
using Expresso.RequestTypes;

namespace Carsales.Web.Features.Home.Makes
{
    public class MakesViewModelItem
    {
        public string Name { get; set; }
        public long Count { get; set; }
    }

    public static class MakesRenderPartialExtensions
    {
        public static void RenderPartialForMakesWithCount(this HtmlHelper html)
        {
            html.RenderPartial("~/Features/Home/Makes/Views/Index.cshtml", 
                DependencyResolver.Current.GetService<IContextStore<IEnumerable<MakesViewModelItem>>>().Get());
        }
    }

    
}