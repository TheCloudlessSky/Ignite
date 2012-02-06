using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ignite.Assets;
using Ignite.Processing;
using Moq;

namespace Ignite.Test.Assets
{
    [TestClass]
    public class StyleSheetPackageTest
    {
        [TestMethod]
        public void Should_use_stylesheet_processor_to_get_data()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("content/style/1.css").Data("abc123").Build();
            var a2 = builder.Path("content/style/2.css").Data("def456").Build();
            var processor = new Mock<IStyleSheetProcessor>();
            processor.Setup(p => p.Execute("abc123\r\ndef456\r\n")).Returns("A");
            processor.Setup(p => p.Execute("abc123")).Returns("B");
            var package = this.CreatePackage(new[] { a1, a2 }, processor.Object);

            // Act / Assert
            Assert.AreEqual("A", package.GetData());
            Assert.AreEqual("B", package.GetData("content/style/1.css"));
        }

        private StyleSheetPackage CreatePackage(IList<IAsset> assets, IStyleSheetProcessor processor)
        {
            return new StyleSheetPackage(
                Guid.NewGuid().ToString(),
                assets,
                processor
            );
        }
    }
}
