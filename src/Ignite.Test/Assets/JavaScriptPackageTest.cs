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
    public class JavaScriptPackageTest
    {
        [TestMethod]
        public void Should_not_add_template_asset_if_no_templates_exist()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Data("abc123").Path("content/scripts/1.js").Build();
            var a2 = builder.Data("def456").Path("content/scripts/2.js").Build();
            var a3 = builder.Data("ghi789").Path("content/scripts/3.js").Build();

            // Act
            var package = this.CreatePackage(new List<IAsset>() { a1, a2, a3 }, "jst");

            // Assert
            Assert.AreEqual(3, package.Assets.Count());
            Assert.AreEqual("content/scripts/1.js", package.Assets.First().Path);
            Assert.AreEqual("content/scripts/2.js", package.Assets.Skip(1).First().Path);
            Assert.AreEqual("content/scripts/3.js", package.Assets.Skip(2).First().Path);
        }

        [TestMethod]
        public void Should_aggregate_template_assets()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("content/scripts/1.js").Build();
            var a2 = builder.Path("content/scripts/2.js").Build();
            var a3 = builder.Path("content/tmpl/1.jst").Build();
            var a4 = builder.Path("content/tmpl/2.jst").Build();

            // Act
            var package = this.CreatePackage(new List<IAsset>() { a1, a2, a3, a4 }, "jst");

            // Assert
            Assert.AreEqual(3, package.Assets.Count());
            Assert.AreEqual("content/scripts/1.js", package.Assets.First().Path);
            Assert.AreEqual("content/scripts/2.js", package.Assets.Skip(1).First().Path);

            var tmplAsset = package.Assets.Skip(2).First();
            Assert.IsTrue(tmplAsset.Path.StartsWith("templates/"));
            Assert.IsTrue(tmplAsset.Path.EndsWith(".js"));
            Assert.IsInstanceOfType(tmplAsset, typeof(TemplateAsset));
        }

        private JavaScriptPackage CreatePackage(IList<IAsset> assets, string templateExtension, IJavaScriptProcessor processor = null)
        {
            return new JavaScriptPackage(
                Guid.NewGuid().ToString(), 
                assets, 
                new Mock<IJavaScriptProcessor>().Object, 
                new Mock<IDebugState>().Object,
                new TemplateConfiguration() { Extension = templateExtension }
            );
        }
    }
}
