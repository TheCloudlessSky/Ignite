using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ignite.Processing;

namespace Ignite.Assets
{
    public class JavaScriptPackage : PackageBase
    {
        private readonly IJavaScriptProcessor processor;

        public JavaScriptPackage(string name, IList<IAsset> assets, IJavaScriptProcessor processor, TemplateConfiguration configuration)
            : base(name, assets)
        {
            this.processor = processor;
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

        protected override string Process(string data)
        {
            return this.processor.Execute(data);
        }
    }
}
