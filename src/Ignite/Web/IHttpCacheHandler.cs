using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Ignite.Web
{
    public interface IHttpCacheHandler
    {
        void ProcessRequest(HttpContextBase httpContext); 
    }
}
