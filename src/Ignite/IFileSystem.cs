using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite
{
    public interface IFileSystem
    {
        string ReadFile(string absolutePath);
        IEnumerable<string> EnumerateFiles(string path, string searchPattern, System.IO.SearchOption searchOption);
    }
}
