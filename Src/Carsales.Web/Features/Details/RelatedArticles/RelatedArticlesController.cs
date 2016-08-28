using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Bolt.Common.Extensions;
using Carsales.Web.Infrastructure.Stores;

namespace Carsales.Web.Features.Details.RelatedArticles
{
    [RoutePrefix("details/child-action/related-articles")]
    public class RelatedArticlesController : Controller
    {
        private readonly IContextStore<IEnumerable<RelatedArticleViewModelItem>> context;

        public RelatedArticlesController(IContextStore<IEnumerable<RelatedArticleViewModelItem>> context)
        {
            this.context = context;
        }

        [Route]
        [ChildActionOnly]
        public ActionResult Index()
        {
            var vm = context.Get().NullSafe();
            return PartialView("~/Features/Details/RelatedArticles/Views/Index.cshtml", vm);
        }
    }

    public class RelatedArticleViewModelItem
    {
        public string Title { get; set; }
        public string Photo { get; set; }
    }
}