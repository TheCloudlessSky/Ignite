using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ignite.Assets;
using Ignite.Processing;
using System.Web;
using System.Web.Routing;
using Ignite.Web;
using Ignite.Html;
using System.Diagnostics.Contracts;
using System.Web.Hosting;
using System.Web.UI;

namespace Ignite
{
    // TODO: Test with /assets/mos.core.js?v=1231324314143 (period in name).
    public class PackageContainer : IPackageContainerInternal
    {
        // TODO: Allow mutliple package containers?
        internal const string JavaScriptExtension = "js";
        internal const string StyleSheetExtension = "css";
        internal const string RouteValue = "package";
        public const string DebugQueryParam = "debug";
        public const string VersionQueryParam = "v";

        private static object syncRoot = new object();
        private static IPackageContainerInternal current;

        private bool isBuilt;
        private string routePrefix;
        private readonly IAssetResolver resolver;
        private readonly TemplateConfiguration templateConfig;
        private readonly string appPath;
        internal readonly Dictionary<string, IPackage> javascripts = new Dictionary<string, IPackage>();
        internal readonly Dictionary<string, IPackage> stylesheets = new Dictionary<string, IPackage>();
        private readonly DebugState debugState;
        private ITagRenderer tagRenderer;
        private IVersionGenerator versionGenerator;
        private IJavaScriptProcessor javascriptProcessor;
        private IStyleSheetProcessor stylesheetProcessor;

        public IDebugState DebugState { get { return this.debugState; } }
        public ITagRenderer TagRenderer { get { return this.tagRenderer; } }
        public IHttpCacheHandler CacheHandler { get; private set; }
        public IAssetResultWriter Writer { get; private set; }

        internal PackageContainer()
        {
            this.routePrefix = "assets";
            this.appPath = HttpRuntime.AppDomainAppPath;

            this.templateConfig = new TemplateConfiguration()
            {
                Function = "_.template",
                Extension = "jst",
                Namespace = "JST"
            };

            this.debugState = new DebugState();
            this.versionGenerator = new HashedVersionGenerator();
            this.javascriptProcessor = new YuiJavaScriptProcessor();
            this.stylesheetProcessor = new DotLessStyleSheetProcessor(this.appPath);

            this.resolver = new AssetResolver(new FileSystemWrapper());
        }

        public IPackageContainer JavaScript(string name, string[] include, string[] exclude = null)
        {
            var assets = this.resolver.GetAssets(this.appPath, include, exclude ?? Enumerable.Empty<string>());
            var js = new JavaScriptPackage(name, assets, this.javascriptProcessor, this.debugState, this.templateConfig);
            this.javascripts.Add(name, new CachedPackage(js, this.debugState));
            return this;
        }

        public IPackageContainer StyleSheet(string name, string[] include, string[] exclude = null)
        {
            var assets = this.resolver.GetAssets(this.appPath, include, exclude ?? Enumerable.Empty<string>());
            var style = new StyleSheetPackage(name, assets, this.stylesheetProcessor, this.debugState);
            this.stylesheets.Add(name, new CachedPackage(style, this.debugState));
            return this;
        }

        public void Map(RouteCollection routes)
        {
            Contract.Requires(!this.isBuilt, "The container is already built.");

            var appPath = HostingEnvironment.ApplicationVirtualPath;
            var tagRenderer = new TagRenderer(appPath + this.routePrefix, this.javascripts, this.stylesheets, this.versionGenerator, this.debugState);
            this.tagRenderer = new CachedTagRenderer(tagRenderer, this.debugState);

            this.Writer = new CachedGZipAssetResultWriter(this.CacheHandler);

            routes.Add(new Route(this.routePrefix + "/{" + RouteValue + "}", new IgniteRouteHandler(this)));
            this.isBuilt = true;
        }

        public IAssetResult GetAsset(RequestContext requestContext)
        {
            Contract.Requires(this.isBuilt, "The container must be built before requesting assets.");

            var packageRouteData = requestContext.RouteData.GetRequiredString(RouteValue).Split('.');

            var name = String.Join(".", packageRouteData.TakeWhile((c, i) => i < packageRouteData.Length - 1));
            var extension = packageRouteData.Last();

            var result = new AssetResult();
            IPackage package = null;

            switch (extension.ToLower())
            {
                case JavaScriptExtension:
                    result.ContentType = "text/javascript";
                    package = this.javascripts[name];
                    break;
                case StyleSheetExtension:
                    result.ContentType = "text/css";
                    package = this.stylesheets[name];
                    break;
                default:
                    return null;
            }

            var debugQueryParam = requestContext.HttpContext.Request.QueryString[PackageContainer.DebugQueryParam];

            if (this.debugState.IsDebugging() && !String.IsNullOrEmpty(debugQueryParam))
            {
                result.Data = package.GetAssetData(debugQueryParam);
            }
            else
            {
                result.Data = package.GetAllData();
            }

            return result;
        }

        public IPackageContainer DisableHttpCaching()
        {
            this.CacheHandler = null;
            return this;
        }

        public IPackageContainer DisableDebugging()
        {
            this.debugState.Disable();
            return this;
        }

        public IPackageContainer EnableHttpCaching()
        {
            return this.EnableHttpCaching((int)TimeSpan.FromDays(365).TotalSeconds);
        }

        public IPackageContainer EnableHttpCaching(int cacheDuration)
        {
            var cacheParams = new OutputCacheParameters()
            {
                Duration = cacheDuration,
                VaryByParam = PackageContainer.VersionQueryParam
            };
            return this.EnableHttpCaching(new PageBasedHttpCacheHandler(this.debugState, cacheParams));
        }

        public IPackageContainer EnableHttpCaching(IHttpCacheHandler cacheHandler)
        {
            Contract.Requires(cacheHandler != null);
            this.CacheHandler = cacheHandler;
            return this;
        }

        public IPackageContainer EnableDebugging()
        {
            this.debugState.Enable();
            return this;
        }

        public IPackageContainer JavaScriptProcessor(IJavaScriptProcessor processor)
        {
            Contract.Requires(processor != null);
            this.javascriptProcessor = processor;
            return this;
        }

        public IPackageContainer StyleSheetProcessor(IStyleSheetProcessor processor)
        {
            Contract.Requires(processor != null);
            this.stylesheetProcessor = processor;
            return this;
        }

        public IPackageContainer TemplateExtension(string templateFileExtension)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(templateFileExtension));
            this.templateConfig.Extension = templateFileExtension;
            return this;
        }

        public IPackageContainer TemplateFunction(string templateFunctionName)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(templateFunctionName));
            this.templateConfig.Function = templateFunctionName;
            return this;
        }

        public IPackageContainer TemplateNamespace(string templateNamespace)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(templateNamespace));
            this.templateConfig.Namespace = templateNamespace;
            return this;
        }

        public IPackageContainer RoutePrefix(string routePrefix)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(routePrefix));
            this.routePrefix = routePrefix;
            return this;
        }

        public IPackageContainer VersionGenerator(IVersionGenerator versionGenerator)
        {
            Contract.Requires(versionGenerator != null);
            this.versionGenerator = versionGenerator;
            return this;
        }

        public static IPackageContainer Create()
        {
            lock (syncRoot)
            {
                if (current != null)
                {
                    throw new InvalidOperationException("Only one pacakge container can be created.");
                }
                current = new PackageContainer();
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
