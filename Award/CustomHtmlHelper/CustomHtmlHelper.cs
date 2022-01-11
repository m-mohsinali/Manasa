using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Award.Web.CustomHtmlHelper
{
    public static class CustomHtmlHelper
    {
        public static HtmlString Image(this HtmlHelper helper,string src,string alt )
        {
            TagBuilder tb = new TagBuilder("img");
            tb.Attributes.Add("src", src);
            tb.Attributes.Add("alt", alt);
            tb.TagRenderMode = TagRenderMode.SelfClosing;
            return new HtmlString( tb.ToString());
        }

    }
}
