﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Assets
{
    public interface IPackage
    {
        IEnumerable<IAsset> Assets { get; }
        string GetData();
        string GetData(string assetPath);
    }
}
