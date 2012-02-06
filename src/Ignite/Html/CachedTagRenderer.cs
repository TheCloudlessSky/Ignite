using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Html
{
    public class CachedTagRenderer : ITagRenderer
    {
        private readonly Dictionary<Tuple<string, object>, string> javascriptCache = new Dictionary<Tuple<string, object>, string>();
        private readonly Dictionary<Tuple<string, object>, string> stylesheetCache = new Dictionary<Tuple<string, object>, string>();
        private readonly ITagRenderer renderer;
        private readonly IDebugState debugState;
        private readonly object syncRoot = new object();

        public CachedTagRenderer(ITagRenderer renderer, IDebugState debugState)
        {
            this.renderer = renderer;
            this.debugState = debugState;
        }

        public string JavaScriptTag(string name, object htmlAttributes)
        {
            var key = Tuple.Create(name, htmlAttributes ?? new { });
            lock (this.syncRoot)
            {
                if (this.debugState.IsDebugging() || !this.javascriptCache.ContainsKey(key))
                {
                    this.javascriptCache[key] = this.renderer.JavaScriptTag(name, htmlAttributes);
                }
            }
            return this.javascriptCache[key];
        }

        public string StyleSheetTag(string name, object htmlAttributes)
        {
            var key = Tuple.Create(name, htmlAttributes ?? new { });
            lock (this.syncRoot)
            {
                if (this.debugState.IsDebugging() || !this.stylesheetCache.ContainsKey(key))
                {
                    this.stylesheetCache[key] = this.renderer.StyleSheetTag(name, htmlAttributes);
                }
            }
            return this.stylesheetCache[key];
        }
    }
}
