using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Assets
{
    public class CachedPackage : IPackage
    {
        private readonly IDebugState debugState;
        private readonly IPackage package;
        private string cachedData;
        private readonly Dictionary<string, string> cachedByPath = new Dictionary<string, string>();
        private readonly object syncRoot = new object();

        public IEnumerable<IAsset> Assets
        {
            get { return this.package.Assets; }
        }

        public CachedPackage(IPackage package, IDebugState debugState)
        {
            this.package = package;
            this.debugState = debugState;
        }

        public string GetAllData()
        {
            lock (this.syncRoot)
            {
                if (this.debugState.IsDebugging() || this.cachedData == null)
                {
                    this.cachedData = this.package.GetAllData();
                }
            }

            return this.cachedData;
        }

        public string GetAssetData(string assetPath)
        {
            lock (this.syncRoot)
            {
                if (this.debugState.IsDebugging() || !this.cachedByPath.ContainsKey(assetPath))
                {
                    this.cachedByPath[assetPath] = this.package.GetAssetData(assetPath);
                }
            }
            return this.cachedByPath[assetPath];
        }
    }
}
