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

    public interface IMakesProvider
    {
        IEnumerable<MakesViewModelItem> Get();
        void Set(IEnumerable<MakesViewModelItem> vm);
    }

    [AutoBind]
    public class MakesProvider : IMakesProvider
    {
        private const string Key = "MakesProvider:MakesViewModelItem";
        private readonly IContextStore store;

        public MakesProvider(IContextStore store)
        {
            this.store = store;
        }

        public IEnumerable<MakesViewModelItem> Get()
        {
            return store.Get<IEnumerable<MakesViewModelItem>>(Key).NullSafe();
        }

        public void Set(IEnumerable<MakesViewModelItem> vm)
        {
            store.Set(Key, vm);
        }
    }

    public static class MakesRenderPartialExtensions
    {
        public static void RenderPartialForMakesWithCount(this HtmlHelper html)
        {
            html.RenderPartial("~/Features/Home/Makes/Views/Index.cshtml", 
                DependencyResolver.Current.GetService<IMakesProvider>().Get());
        }
    }

    public class SelectListResponseDto
    {
        public int Count { get; set; }
        public SelectListCollection SelectListCollection { get; set; }
    }
}