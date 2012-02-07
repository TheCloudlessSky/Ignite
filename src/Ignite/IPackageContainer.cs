using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ignite.Processing;
using System.Web.Routing;
using Ignite.Web;
using Ignite.Html;
using System.Diagnostics.Contracts;

namespace Ignite
{
    internal interface IPackageContainerInternal : IPackageContainer
    {
        ITagRenderer TagRenderer { get; }
        IAssetResultWriter Writer { get; }
        IAssetResult GetAsset(RequestContext context);
    }

    public interface IPackageContainer
    {
        IDebugState DebugState { get; }

        /// <summary>
        /// Adds a new JavaScript package.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="include"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        IPackageContainer JavaScript(string name, string[] include, string[] exclude = null);

        /// <summary>
        /// Adds a new StyleSheet package.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="include"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        IPackageContainer StyleSheet(string name, string[] include, string[] exclude = null);

        /// <summary>
        /// Disables HTTP caching.
        /// </summary>
        /// <returns></returns>
        IPackageContainer DisableCaching();

        /// <summary>
        /// Enables HTTP caching for one year.
        /// </summary>
        /// <returns></returns>
        IPackageContainer EnableCaching();

        /// <summary>
        /// Enables HTTP caching for the specified duration.
        /// </summary>
        /// <param name="cacheDuration"></param>
        /// <returns></returns>
        IPackageContainer EnableCaching(int cacheDuration);

        /// <summary>
        /// Enables HTTP caching using the specified cache handler.
        /// </summary>
        /// <param name="cacheHandler"></param>
        /// <returns></returns>
        IPackageContainer EnableCaching(IHttpCacheHandler cacheHandler);

        /// <summary>
        /// Disabling debugging combines assets into a single HTML tag and enables internal file caching.
        /// JavaScript and StyleSheet files are fully processed.
        /// </summary>
        /// <returns></returns>
        IPackageContainer DisableDebugging();

        /// <summary>
        /// Enabling debugging writes each asset as an individual HTML tag as well as disables
        /// internal file caching. JavaScript and StyleSheet files should not be processed.
        /// </summary>
        /// <returns></returns>
        IPackageContainer EnableDebugging();

        /// <summary>
        /// Sets the processor used for JavaScript assets.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        IPackageContainer JavaScriptProcessor(IJavaScriptProcessor processor);

        /// <summary>
        /// Sets the processor used for StyleSheet assets.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        IPackageContainer StyleSheetProcessor(IStyleSheetProcessor processor);

        /// <summary>
        /// Sets the file extension used for filtering JavaScript files to templates. The default value is 'jst'.
        /// </summary>
        /// <param name="templateFileExtension"></param>
        /// <returns></returns>
        IPackageContainer TemplateExtension(string templateFileExtension);

        /// <summary>
        /// Sets the template function used for building templates on the client. The default value is '_.template'.
        /// </summary>
        /// <param name="templateFunctionName"></param>
        /// <returns></returns>
        IPackageContainer TemplateFunction(string templateFunctionName);

        /// <summary>
        /// Sets the template namespace in which all templates are stored on the 'window' object. The default value is 'JST'.
        /// </summary>
        /// <param name="templateNamespace"></param>
        /// <returns></returns>
        IPackageContainer TemplateNamespace(string templateNamespace);

        /// <summary>
        /// Sets the route prefix used for the packaged assets. The default value is "assets". 
        /// For example, setting this to "static" will result in serving files like "/static/core.js".
        /// </summary>
        /// <param name="routePrefix"></param>
        /// <returns></returns>
        IPackageContainer RoutePrefix(string routePrefix);

        /// <summary>
        /// Sets the generator used to generate the unique identifiers 
        /// </summary>
        /// <param name="versionGenerator"></param>
        /// <returns></returns>
        IPackageContainer VersionGenerator(IVersionGenerator versionGenerator);

        void Build(RouteCollection routes);
    }
}
