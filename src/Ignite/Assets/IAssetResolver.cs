using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Assets
{
    public interface IAssetResolver
    {
        IList<IAsset> GetAssets(string appPath, IEnumerable<string> include, IEnumerable<string> exclude);
    }
}
