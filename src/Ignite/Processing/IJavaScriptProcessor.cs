using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Processing
{
    public interface IJavaScriptProcessor
    {
        string Execute(string data);
    }
}
