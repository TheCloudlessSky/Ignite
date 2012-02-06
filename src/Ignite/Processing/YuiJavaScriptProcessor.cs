using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yahoo.Yui.Compressor;

namespace Ignite.Processing
{
    public class YuiJavaScriptProcessor : IJavaScriptProcessor
    {
        private readonly IDebugState debugState;

        public YuiJavaScriptProcessor(IDebugState debugState)
        {
            this.debugState = debugState;
        }

        public string Execute(string data)
        {
            if (this.debugState.IsDebugging())
            {
                return data;
            }
            else
            {
                return JavaScriptCompressor.Compress(data, true);
            }
        }
    }
}
