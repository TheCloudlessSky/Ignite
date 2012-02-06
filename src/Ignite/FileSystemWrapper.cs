using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite
{
    internal class FileSystemWrapper : IFileSystem
    {
        public string ReadFile(string absolutePath)
        {
            return System.IO.File.ReadAllText(absolutePath);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            return System.IO.Directory.EnumerateFiles(path, searchPattern, searchOption);
        }
    }
}
