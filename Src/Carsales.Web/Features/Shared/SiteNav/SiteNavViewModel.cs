using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bolt.Common.Extensions;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public class SiteNavViewModel
    {
        public static SiteNavViewModel Empty = new SiteNavViewModel
        {
            Footer = string.Empty,
            Script = string.Empty,
            Style = string.Empty,
            TopNav = string.Empty
        };

        public string Script { get; set; }
        public string Style { get; set; }
        public string TopNav { get; set; }
        public string Footer { get; set; }

        public MvcHtmlString ScriptTag => Script.HasValue() 
            ? MvcHtmlString.Create($"<script src=\"{Script}\"></script>") 
            : MvcHtmlString.Empty;

        public MvcHtmlString StyleTag => Style.HasValue()
            ? MvcHtmlString.Create($"<style href=\"{Style}\" rel=\"stylesheet\" type=\"text/css\"/>")
            : MvcHtmlString.Empty;


        public MvcHtmlString FooterHtml => MvcHtmlString.Create(Footer);
        public MvcHtmlString TopNavHtml => MvcHtmlString.Create(TopNav);
    }
}