using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite
{
    public interface IStyleSheetPackageSyntax
    {
        IStyleSheetPackageSyntax StyleSheet(string name, string[] include, string[] exclude = null);
    }
}
