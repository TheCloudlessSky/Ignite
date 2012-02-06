using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;

namespace Ignite.Web
{
    internal class IgniteRouteHandler : IRouteHandler
    {
        private readonly IPackageContainerInternal container;

        public IgniteRouteHandler(IPackageContainerInternal container)
        {
            this.container = container;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            IAssetResult asset = this.container.GetAsset(requestContext);
            return new IgniteHttpHandler(asset, container.Writer);
        }
    }
}
