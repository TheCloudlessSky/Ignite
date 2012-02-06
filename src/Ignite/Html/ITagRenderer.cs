using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Html
{
    public interface ITagRenderer
    {
        string JavaScriptTag(string name, object htmlAttributes);
        string StyleSheetTag(string name, object htmlAttributes);
    }
}
