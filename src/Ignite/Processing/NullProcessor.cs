using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Processing
{
    public class NullProcessor : IJavaScriptProcessor, IStyleSheetProcessor
    {
        public string Preprocess(string data, string fileName)
        {
            return data;
        }

        public string Process(string data)
        {
            return data;
        }
    }
}
