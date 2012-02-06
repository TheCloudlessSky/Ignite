using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;

namespace Ignite.Web
{
    public class PageBasedHttpCacheHandler : IHttpCacheHandler
    {
        private readonly IDebugState debugState;
        private readonly OutputCacheParameters cacheParams;

        public PageBasedHttpCacheHandler(IDebugState debugState, OutputCacheParameters cacheParams)
        {
            this.debugState = debugState;
            this.cacheParams = cacheParams;
        }

        public void ProcessRequest(HttpContextBase httpContext)
        {
            if (!this.debugState.IsDebugging())
            {
                var page = new FakeCachePage(this.cacheParams);
                page.ProcessRequest(HttpContext.Current);
            }
        }

        private sealed class FakeCachePage : Page
        {
            private readonly OutputCacheParameters cacheSettings;

            public FakeCachePage(OutputCacheParameters cacheSettings)
            {
                this.ID = Guid.NewGuid().ToString();
                this.cacheSettings = cacheSettings;
            }

            protected override void FrameworkInitialize()
            {
                base.FrameworkInitialize();
                this.InitOutputCache(this.cacheSettings);
            }
        }
    }
}
