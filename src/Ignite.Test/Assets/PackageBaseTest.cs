using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ignite.Assets;
using Moq;
using Ignite.Processing;

namespace Ignite.Test.Assets
{
    [TestClass]
    public class PackageBaseTest
    {
        [TestMethod]
        public void Can_get_data_from_all_assets_with_debugging_disabled()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("f/1.js").Data("abc123").Build();
            var a2 = builder.Path("f/2.js").Data("def456").Build();
            var a3 = builder.Path("f/3.js").Data("ghi789").Build();

            var p = this.CreatePackage(new[] { a1, a2, a3 }, isDebugging: false);

            // Act
            var d = p.GetAllData();

            // Assert
            Assert.AreEqual("_$abc123-f/1.js$\r\n$def456-f/2.js$\r\n$ghi789-f/3.js$\r\n_", d);
        }

        [TestMethod]
        public void Can_get_data_from_all_assets_with_debugging_enabled()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("f/1.js").Data("abc123").Build();
            var a2 = builder.Path("f/2.js").Data("def456").Build();
            var a3 = builder.Path("f/3.js").Data("ghi789").Build();

            var p = this.CreatePackage(new[] { a1, a2, a3 }, isDebugging: true);

            // Act
            var d = p.GetAllData();

            // Assert
            Assert.AreEqual("$abc123-f/1.js$\r\n$def456-f/2.js$\r\n$ghi789-f/3.js$\r\n", d);
        }

        [TestMethod]
        public void Can_get_data_from_individual_asset()
        {
            Action<bool> getAssetDataTest = (bool isDebugging) =>
            {
                // Arrange
                var builder = new AssetBuilder();
                var a1 = builder.Path("content/1.js").Data("abc123").Build();
                var a2 = builder.Path("ContenT/2.js").Data("def456").Build();

                var p = this.CreatePackage(new[] { a1, a2 }, isDebugging);

                // Act/Assert
                Assert.AreEqual("$abc123-content/1.js$", p.GetAssetData("content/1.js"));
                Assert.AreEqual("$def456-ContenT/2.js$", p.GetAssetData("cONTENt/2.js"));
                Assert.IsNull(p.GetAssetData("non-existant-path/3.js"));
            };

            // Should return same results when debugging is enabled/disabled.
            // Only preprocessing should execute.
            getAssetDataTest(false);
            getAssetDataTest(true);
        }

        private IPackage CreatePackage(IList<IAsset> assets, bool isDebugging)
        {
            var processor = new Mock<IProcessor>();
            processor.Setup(p => p.Preprocess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string data, string fileName) =>
                {
                    return "$" + data + "-" + fileName + "$";
                });
            processor.Setup(p => p.Process(It.IsAny<string>()))
                .Returns((string data) => "_" + data + "_");

            var debugState = new Mock<IDebugState>();
            debugState.Setup(d => d.IsDebugging()).Returns(isDebugging);

            return new Mock<PackageBase>(Guid.NewGuid().ToString(), assets, processor.Object, debugState.Object).Object;
        }
    }
}
