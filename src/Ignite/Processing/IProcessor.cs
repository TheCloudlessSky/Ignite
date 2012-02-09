using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Processing
{
    public interface IProcessor
    {
        string Preprocess(string data, string fileName);
        string Process(string data);
    }
}
