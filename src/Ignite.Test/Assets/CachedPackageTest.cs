using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ignite.Assets;
using Moq;

namespace Ignite.Test.Assets
{
    [TestClass]
    public class CachedPackageTest
    {
        [TestMethod]
        public void Should_invalidate_when_getting_all_data_if_debugging()
        {
            // Arrange
            var p = this.CreatePackage(true, "abc123", "def456", "ghi789");

            // Act / Assert
            Assert.AreEqual("abc123", p.GetData());
            Assert.AreEqual("def456", p.GetData());
            Assert.AreEqual("ghi789", p.GetData());
        }

        [TestMethod]
        public void Should_invalidate_when_getting_individual_data_if_debugging()
        {
            // Arrange
            var p = this.CreateSinglePackage(true, new[]
            { 
                "content/1.js", "content/2.js"
            },
            new[]
            {
                new [] { "A1", "A2", "A3" },
                new [] { "B1", "B2", "B3" }
            });

            // Act / Assert
            Assert.AreEqual("A1", p.GetData("content/1.js"));
            Assert.AreEqual("A2", p.GetData("content/1.js"));
            Assert.AreEqual("A3", p.GetData("content/1.js"));

            Assert.AreEqual("B1", p.GetData("content/2.js"));
            Assert.AreEqual("B2", p.GetData("content/2.js"));
            Assert.AreEqual("B3", p.GetData("content/2.js"));
        }

        [TestMethod]
        public void Should_cache_when_getting_all_data_and_not_debugging()
        {
            // Arrange
            var p = this.CreatePackage(false, "abc123", "def456", "ghi789");

            // Act / Assert
            Assert.AreEqual("abc123", p.GetData());
            Assert.AreEqual("abc123", p.GetData());
            Assert.AreEqual("abc123", p.GetData());
        }

        [TestMethod]
        public void Should_cache_when_getting_individual_data_and_not_debugging()
        {
            // Arrange
            var p = this.CreateSinglePackage(false, new[]
            { 
                "content/1.js", "content/2.js"
            },
            new[]
            {
                new [] { "A1", "A2", "A3" },
                new [] { "B1", "B2", "B3" }
            });

            // Act / Assert
            Assert.AreEqual("A1", p.GetData("content/1.js"));
            Assert.AreEqual("A1", p.GetData("content/1.js"));
            Assert.AreEqual("A1", p.GetData("content/1.js"));

            Assert.AreEqual("B1", p.GetData("content/2.js"));
            Assert.AreEqual("B1", p.GetData("content/2.js"));
            Assert.AreEqual("B1", p.GetData("content/2.js"));
        }

        private CachedPackage CreatePackage(bool isDebugging, params string[] data)
        {
            var package = new Mock<IPackage>();
            package.Setup(p => p.GetData()).ReturnsInOrder(data);
            
            var debugState = new Mock<IDebugState>();
            debugState.Setup(d => d.IsDebugging()).Returns(isDebugging);

            return new CachedPackage(package.Object, debugState.Object);
        }

        private CachedPackage CreateSinglePackage(bool isDebugging, string[] paths, string[][] data)
        {
            var package = new Mock<IPackage>();

            for (int i = 0; i < paths.Length; i++)
            {
                package.Setup(p => p.GetData(paths[i])).ReturnsInOrder(data[i]);
            }

            var debugState = new Mock<IDebugState>();
            debugState.Setup(d => d.IsDebugging()).Returns(isDebugging);

            return new CachedPackage(package.Object, debugState.Object);
        }
    }
}
