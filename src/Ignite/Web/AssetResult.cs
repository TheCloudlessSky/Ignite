using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Web
{
    public class AssetResult : IAssetResult
    {
        public string ContentType { get; set; }
        public string Data { get; set; }
    }
}
