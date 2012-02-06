using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ignite.Processing;

namespace Ignite.Assets
{
    public class StyleSheetPackage : PackageBase
    {
        private readonly IStyleSheetProcessor processor;

        public StyleSheetPackage(string name, IList<IAsset> assets, IStyleSheetProcessor processor)
            : base(name, assets)
        {
            this.processor = processor;
        }

        protected override string Process(string data)
        {
            return this.processor.Execute(data);
        }
    }
}
