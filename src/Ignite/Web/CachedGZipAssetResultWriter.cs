using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.IO.Compression;

namespace Ignite.Web
{
    public class CachedGZipAssetResultWriter : IAssetResultWriter
    {
        private readonly IHttpCacheHandler cacheHandler;

        public CachedGZipAssetResultWriter(IHttpCacheHandler cacheHandler)
        {
            this.cacheHandler = cacheHandler;
        }

        public void Write(IAssetResult result, HttpContextBase context)
        {
            if (this.cacheHandler != null)
            {
                this.cacheHandler.ProcessRequest(context);
            }

            var response = context.Response;

            // TODO: Cache should vary on Accept-Encoding.

            // Determine if the response can be gzipped.
            string acceptEncoding = context.Request.Headers["Accept-Encoding"];
            bool clientSupportsGzip = (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate"));
            string encoding = clientSupportsGzip ? "gzip" : "utf-8";

            byte[] output = Encoding.UTF8.GetBytes(result.Data);

            if (clientSupportsGzip)
            {
                // Convert the output to a GZip stream.
                using (var stream = new MemoryStream())
                using (var gzStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    gzStream.Write(output, 0, output.Length);
                    gzStream.Close();
                    output = stream.ToArray();
                }
            }

            response.ContentType = result.ContentType;

            response.AppendHeader("Content-Encoding", encoding);
            response.OutputStream.Write(output, 0, output.Length);
        }
    }
}
