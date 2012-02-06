using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Assets
{
    public interface IAsset
    {
        string Path { get; }
        string GetData();
    }
}
