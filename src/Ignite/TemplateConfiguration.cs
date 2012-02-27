using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ignite.Assets;

namespace Ignite
{
    public class TemplateConfiguration
    {
        public string Function { get; set; }
        public string Extension { get; set; }
        public string Namespace { get; set; }
        public Func<string, string> NameCasing { get; set; }

        public TemplateConfiguration()
        {

        }

        public TemplateConfiguration(string tmplNamespace, string tmplFunction, string tmplExtension)
        {
            this.Namespace = tmplNamespace;
            this.Function = tmplFunction;
            this.Extension = tmplExtension;
            this.NameCasing = Casing.Default;
        }
    }
}
