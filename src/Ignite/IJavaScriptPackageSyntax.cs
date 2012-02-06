using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite
{
    public interface IJavaScriptPackageSyntax
    {
        IJavaScriptPackageSyntax JavaScript(string name, string[] include, string[] exclude = null);
    }
}
