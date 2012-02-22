using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Ignite.Assets;

namespace Ignite.Html
{
    public class TagRenderer : ITagRenderer
    {
        private readonly string routeRootPath;
        private readonly IDebugState debugState;
        private readonly IVersionGenerator versionGenerator;
        private readonly Dictionary<string, IPackage> javascripts;
        private readonly Dictionary<string, IPackage> stylesheets;

        public TagRenderer(string routeRootPath, Dictionary<string, IPackage> javascripts, Dictionary<string, IPackage> stylesheets, IVersionGenerator versionGenerator, IDebugState debugState)
        {
            this.routeRootPath = routeRootPath;
            this.javascripts = javascripts;
            this.stylesheets = stylesheets;
            this.debugState = debugState;
            this.versionGenerator = versionGenerator;
        }

        public string JavaScriptTag(string name, object htmlAttributes)
        {
            var package = this.javascripts[name];
            return this.Tag(package, name, PackageContainer.JavaScriptExtension, htmlAttributes, this.RenderJavaScript);
        }

        public string StyleSheetTag(string name, object htmlAttributes)
        {
            var package = this.stylesheets[name];
            return this.Tag(package, name, PackageContainer.StyleSheetExtension, htmlAttributes, this.RenderStyleSheet);
        }

        private string Tag(IPackage package, string name, string extension, object htmlAttributes, Func<string, IDictionary<string, object>, string> renderer)
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            string tag = null;

            if (this.debugState.IsDebugging())
            {
                var paths = package.Assets.Select(a => this.GetPath(name, extension, a.Path));
                tag = String.Join(Environment.NewLine, paths.Select(p => renderer(p, attributes)));
            }
            else
            {
                var path = this.GetPath(name, extension, null);
                tag = renderer(path, attributes);
            }

            return tag + Environment.NewLine;
        }

        private string GetPath(string name, string extension, string debugPath)
        {
            var p = this.routeRootPath + "/" + name + "." + extension + "?";

            if (debugPath != null)
            {
                p += PackageContainer.DebugQueryParam + "=" + debugPath;
            }
            else
            {
                p += PackageContainer.VersionQueryParam + "=" + this.versionGenerator.Generate();
            }

            return p;
        }

        private string RenderJavaScript(string path, IDictionary<string, object> htmlAttributes)
        {
            var sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\" src=\"" + path + "\"");
            sb.Append(this.GetAttributes(htmlAttributes, new HashSet<string>() { "src", "type" }));
            sb.Append("></script>");

            return sb.ToString();
        }

        private string RenderStyleSheet(string path, IDictionary<string, object> htmlAttributes)
        {
            var sb = new StringBuilder();
            sb.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + path + "\"");

            sb.Append(this.GetAttributes(htmlAttributes, new HashSet<string>() { "rel", "href", "type" }));
            sb.Append(" />");
            return sb.ToString();
        }

        private string GetAttributes(IDictionary<string, object> htmlAttributes, ISet<string> skippedAttributes)
        {
            var attributes = String.Join(" ", htmlAttributes
                .Where(kvp => !skippedAttributes.Contains(kvp.Key))
                .Select(kvp => kvp.Key + "=\"" + kvp.Value + "\""));

            return attributes != "" ? (" " + attributes) : "";
        }
    }
}
