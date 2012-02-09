using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yahoo.Yui.Compressor;

namespace Ignite.Processing
{
    public class YuiJavaScriptProcessor : IJavaScriptProcessor
    {
        public string Preprocess(string data, string fileName)
        {
            return data;    
        }

        public string Process(string data)
        {
            return JavaScriptCompressor.Compress(data, true);
        }
    }
}
