using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ignite.Assets;

namespace Ignite.Test.Assets
{
    [TestClass]
    public class AssetResolverTest
    {
        [TestMethod]
        public void Should_combine_paths_of_app_and_script_and_exclude_other_explicit_paths()
        {
            var fs = new Mock<IFileSystem>();
            fs.Setup(f => f.ReadFile("C:\\app\\content\\scripts\\1.js")).Returns("123");
            fs.Setup(f => f.ReadFile("C:\\app\\content\\scripts\\2.js")).Returns("456");

            var b = new AssetResolver(fs.Object);
            var result = b.GetAssets("C:\\app\\", 
                new[] { 
                    "content/scripts/1.js",  // Normal path
                    "/content/scripts/2.js", // Leading slash.
                    "content/scripts/3.js" 
                }, 
                new[] { "content/scripts/3.js" });

            Assert.AreEqual("123", result[0].GetData());
            Assert.AreEqual("content/scripts/1.js", result[0].Path);
            Assert.AreEqual("456", result[1].GetData());
            Assert.AreEqual("content/scripts/2.js", result[1].Path);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Should_resolve_file_wildcards()
        {
            var fs = new Mock<IFileSystem>();
            fs.Setup(f => f.EnumerateFiles("C:\\app\\content\\scripts", "*.js", System.IO.SearchOption.TopDirectoryOnly))
                .Returns(new[] { "C:\\app\\content\\scripts\\1.js", "C:\\app\\content\\scripts\\2.js" });

            fs.Setup(f => f.EnumerateFiles("C:\\app\\content\\other", "*.js", System.IO.SearchOption.TopDirectoryOnly))
                .Returns(new[] { "C:\\app\\content\\other\\1.js" });

            var b = new AssetResolver(fs.Object);
            var result = b.GetAssets("C:\\app\\",
                new[] 
                { 
                    "content/scripts/*.js",
                    "content/other/*.js"
                },
                new[] 
                { 
                    "content/other/*.js" 
                });

            Assert.AreEqual("content/scripts/1.js", result[0].Path);
            Assert.AreEqual("content/scripts/2.js", result[1].Path);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Should_resolve_recursive_file_wildcards()
        {
            var fs = new Mock<IFileSystem>();
            fs.Setup(f => f.EnumerateFiles("C:\\app\\content", "*.js", System.IO.SearchOption.AllDirectories))
                .Returns(new[] { "C:\\app\\content\\scripts\\1.js", "C:\\app\\content\\scripts\\2.js" });

            var b = new AssetResolver(fs.Object);
            var result = b.GetAssets("C:\\app\\",
                new[] 
                { 
                    "content/**/*.js"
                },
                new string[0]);

            Assert.AreEqual("content/scripts/1.js", result[0].Path);
            Assert.AreEqual("content/scripts/2.js", result[1].Path);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Should_not_add_duplicate_paths()
        {
            var fs = new Mock<IFileSystem>();
            fs.Setup(f => f.EnumerateFiles("C:\\app\\content\\scripts", "*.js", System.IO.SearchOption.TopDirectoryOnly))
                .Returns(new[] 
                { 
                    "C:\\app\\content\\scripts\\A.js", 
                    "C:\\app\\content\\scripts\\Z.js" 
                });

            var b = new AssetResolver(fs.Object);
            var result = b.GetAssets("C:\\app\\",
                new[] { 
                    "/content/scripts/Z.js",
                    "/content/scripts/*.js"
                },
                new string[0]);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("content/scripts/Z.js", result[0].Path);
            Assert.AreEqual("content/scripts/A.js", result[1].Path);
        }
    }
}
