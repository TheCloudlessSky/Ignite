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

        public IEnumerable<IAsset> Assets { get { return this.assets; } }
        public string Name { get; private set; }

        protected PackageBase(string name, IList<IAsset> assets)
        {
            this.Name = name;
            this.assets = assets;
        }

        public string GetData()
        {
            var sb = new StringBuilder();

            foreach (var a in this.assets)
            {
                sb.AppendLine(a.GetData());
            }

            return this.Process(sb.ToString());
        }

        public string GetData(string assetPath)
        {
            var asset = this.assets.SingleOrDefault(a => a.Path.Equals(assetPath, StringComparison.CurrentCultureIgnoreCase));

            if (asset == null) { return null; }

            return this.Process(asset.GetData());
        }

        protected abstract string Process(string data);
    }
}
