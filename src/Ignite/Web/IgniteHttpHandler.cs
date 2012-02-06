using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Ignite.Web
{
    public class IgniteHttpHandler : IHttpHandler
    {
        private readonly IAssetResult result;
        private readonly IAssetResultWriter writer;

        public bool IsReusable { get { return false; } }

        public IgniteHttpHandler(IAssetResult result, IAssetResultWriter writer)
        {
            this.result = result;
            this.writer = writer;
        }

        public void ProcessRequest(HttpContext context)
        {
            if (this.result == null || this.result.Data == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            this.writer.Write(this.result, new HttpContextWrapper(context));
        }
    }
}
