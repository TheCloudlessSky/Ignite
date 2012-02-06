using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Ignite.Web
{
    public interface IAssetResultWriter
    {
        void Write(IAssetResult result, HttpContextBase context);
    }
}
