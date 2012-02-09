using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ignite.Processing;

namespace Ignite.Assets
{
    public class JavaScriptPackage : PackageBase
    {
        public JavaScriptPackage(string name, IList<IAsset> assets, IJavaScriptProcessor processor, IDebugState debugState, TemplateConfiguration configuration)
            : base(name, assets, processor, debugState)
        {
            this.InitializeTemplateAssets(configuration);
        }

        private void InitializeTemplateAssets(TemplateConfiguration configuration)
        {
            var tmplAssets = this.assets
                .Where(a => a.Path.EndsWith("." + configuration.Extension))
                .ToList();

            foreach (var asset in tmplAssets)
            {
                this.assets.Remove(asset);
            }

            if (tmplAssets.Count > 0)
            {
                this.assets.Add(new TemplateAsset("templates/" + Guid.NewGuid().ToString().ToLower() + ".js", tmplAssets, configuration));
            }
        }
    }
}
