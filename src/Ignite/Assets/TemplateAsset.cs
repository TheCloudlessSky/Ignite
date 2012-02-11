using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Assets
{
    public class TemplateAsset : IAsset
    {
        private readonly IList<IAsset> templates;
        private readonly TemplateConfiguration configuration;

        public string Path { get; private set; }

        public TemplateAsset(string path, IList<IAsset> templates, TemplateConfiguration configuration)
        {
            this.Path = path;
            this.templates = templates;
            this.configuration = configuration;
        }

        public string GetData()
        {
            var prefix = @"(function(root) { 
  var ns = root.#Namespace# || (root.#Namespace# = {}); 

  var lazyTemplate = function(tmpl) { 
    var compiled = null; 

    return function() { 
      compiled || (compiled = #Function#(tmpl)); 
      return compiled.apply(this, arguments); 
    }; 
  };";
            prefix = prefix.Replace("#Namespace#", this.configuration.Namespace)
                           .Replace("#Function#", this.configuration.Function);

            var item = "  ns[\"{0}\"] = lazyTemplate('{1}');";

            var sb = new StringBuilder();
            sb.AppendLine(prefix);

            var commonPrefix = this.CommonPrefix(new SortedSet<string>(this.templates.Select(a => a.Path)));

            foreach (var tmpl in this.templates)
            {
                var path = tmpl.Path.Replace("." + this.configuration.Extension, "");
                path = path.Substring(commonPrefix);

                if (this.configuration.UseLowerCaseNames)
                {
                    path = path.ToLower();
                }

                var data = tmpl.GetData().Replace(Environment.NewLine, "\\n").Replace("'", "\\'");
                sb.AppendLine(String.Format(item, path, data));
            }

            var suffix = @"})(this);";
            sb.AppendLine(suffix);

            return sb.ToString();
        }

        private int CommonPrefix(ISet<string> paths)
        {
            if (paths.Count <= 1)
            {
                return 0;
            }

            var first = paths.First().Split('/');
            var last = paths.Last().Split('/');

            int i = 0;

            while (first[i] == last[i] && i <= first.Length)
            {
                i++;
            }

            // +1 to account for the '/'.
            var result = String.Join("/", first.Take(i));
            return String.IsNullOrEmpty(result) ? 0 : result.Length + 1;
        }
    }
}
