using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Assets
{
    internal class AssetResolver : IAssetResolver
    {
        private readonly IFileSystem fileSystem;

        public AssetResolver(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public IList<IAsset> GetAssets(string appPath, IEnumerable<string> included, IEnumerable<string> excluded)
        {
            var paths = included
                .SelectMany(i => this.ResolvePath(appPath, i))
                .Distinct()
                .ToList();

            foreach (var e in excluded.SelectMany(e => this.ResolvePath(appPath, e)))
            {
                paths.Remove(e);
            }

            return paths.Select(p =>
            {
                var ap = p.Replace(appPath, "").Replace('\\', '/');
                return (IAsset)new Asset(ap, p, this.fileSystem);
            }).ToList();
        }

        private IEnumerable<string> ResolvePath(string appPath, string path)
        {
            var absoluteParts = System.IO.Path.Combine(appPath, path.Replace('/', '\\').Trim('\\')).Split('\\');

            // Handle wildcards: content/scripts/*.js
            if (absoluteParts.Last().Contains('*'))
            {
                string searchPath = "";
                string searchPattern = absoluteParts.Last();
                var searchOption = System.IO.SearchOption.TopDirectoryOnly;

                for (int i = 0; i < absoluteParts.Length - 1; i++)
                {
                    // Handle recursive searching: content/scripts/**/*.js
                    if (i == absoluteParts.Length - 2 && absoluteParts[i] == "**")
                    {
                        searchOption = System.IO.SearchOption.AllDirectories;
                    }
                    else
                    {
                        searchPath += absoluteParts[i] + '\\';
                    }
                }

                return this.fileSystem.EnumerateFiles(searchPath.TrimEnd('\\'), searchPattern, searchOption);
            }
            else
            {
                return new[] { String.Join("\\", absoluteParts) };
            }
        }
    }
}
