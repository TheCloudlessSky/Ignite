using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Processing
{
    public class NullProcessor : IJavaScriptProcessor, IStyleSheetProcessor
    {
        public string Execute(string data)
        {
            return data;
        }
    }
}
