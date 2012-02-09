using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ignite.Processing;

namespace Ignite.Assets
{
    public class StyleSheetPackage : PackageBase
    {
        public StyleSheetPackage(string name, IList<IAsset> assets, IStyleSheetProcessor processor, IDebugState debugState)
            : base(name, assets, processor, debugState)
        {

        }
    }
}
