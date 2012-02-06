using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Collections.Concurrent;
using System.Web.Mvc;
using System.Security.Cryptography;
using Ignite.Processing;

namespace Ignite
{
    public static class Ignite
    {
        // TODO: Allow mutliple package containers?

        internal const string JavaScriptExtension = "js";
        internal const string StyleSheetExtension = "css";
        internal const string RouteValue = "package";
        public const string DebugQueryParam = "debug";
        public const string VersionQueryParam = "v";

        private static object syncRoot = new object();
        private static IPackageContainerInternal current;

        public static IPackageContainer Create(string routePrefix)
        {
            lock (syncRoot)
            {
                if (current != null)
                {
                    throw new InvalidOperationException("Only one pacakge container can be created.");
                }
                current = new PackageContainer(routePrefix);
            }
            return current;
        }

        public static IHtmlString JavaScriptTag(string name, object htmlAttributes = null)
        {
            return new HtmlString(current.TagRenderer.JavaScriptTag(name, htmlAttributes));
        }

        public static IHtmlString StyleSheetTag(string name, object htmlAttributes = null)
        {
            return new HtmlString(current.TagRenderer.StyleSheetTag(name, htmlAttributes));
        }
    }
}
