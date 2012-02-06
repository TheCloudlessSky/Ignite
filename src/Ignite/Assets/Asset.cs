using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Assets
{
    internal class Asset : IAsset
    {
        private readonly IFileSystem fileSystem;

        public string Path { get; private set; }
        public string FileSystemPath { get; private set; }

        public Asset(string path, string fileSystemPath, IFileSystem fileSystem)
        {
            this.Path = path;
            this.FileSystemPath = fileSystemPath;
            this.fileSystem = fileSystem;
        }

        public string GetData()
        {
            return fileSystem.ReadFile(this.FileSystemPath);
        }
    }
}
