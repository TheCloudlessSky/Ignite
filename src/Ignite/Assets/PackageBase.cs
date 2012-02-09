using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ignite.Processing;

namespace Ignite.Assets
{
    public abstract class PackageBase : IPackage
    {
#pragma warning disable 3005
        protected readonly IList<IAsset> assets;
#pragma warning restore 3005
        protected readonly IDebugState debugState;
        protected readonly IProcessor processor;

        public IEnumerable<IAsset> Assets { get { return this.assets; } }
        public string Name { get; private set; }

        protected PackageBase(string name, IList<IAsset> assets, IProcessor processor, IDebugState debugState)
        {
            this.Name = name;
            this.assets = assets;
            this.processor = processor;
            this.debugState = debugState;
        }

        public string GetAllData()
        {
            var sb = new StringBuilder();

            foreach (var a in this.assets)
            {
                sb.AppendLine(this.processor.Preprocess(a.GetData(), a.Path));
            }

            var data = sb.ToString();
            return this.debugState.IsDebugging() ? data : this.processor.Process(data);
        }

        public string GetAssetData(string assetPath)
        {
            var asset = this.assets.SingleOrDefault(a => a.Path.Equals(assetPath, StringComparison.CurrentCultureIgnoreCase));

            if (asset == null) { return null; }

            return this.processor.Preprocess(asset.GetData(), asset.Path);
        }
    }
}
