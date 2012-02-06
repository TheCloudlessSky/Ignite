using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Web
{
    public interface IAssetResult
    {
        string ContentType { get; }
        string Data { get; }
    }
}
