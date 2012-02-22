using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ignite.Html;
using Moq;
using Ignite.Assets;
using System.Web.Hosting;

namespace Ignite.Test.Html
{
    [TestClass]
    public class TagRendererTest
    {
        [TestMethod]
        public void Can_render_javascript_tag_for_package_when_not_debugging()
        {
            // Arrange
            var renderer = this.CreateTagRenderer("/assets", new[] { "1", "2" }, false, new Dictionary<string, IPackage>()
            {
                {"core", this.CreatePackage()},
                {"ie", this.CreatePackage()}
            });

            // Act
            var tag1 = renderer.JavaScriptTag("core", new object());
            var tag2 = renderer.JavaScriptTag("ie", new object());

            // Assert
            Assert.AreEqual("<script type=\"text/javascript\" src=\"/assets/core.js?v=1\"></script>\r\n", tag1);
            Assert.AreEqual("<script type=\"text/javascript\" src=\"/assets/ie.js?v=2\"></script>\r\n", tag2);
        }

        [TestMethod]
        public void Can_render_javascript_tags_for_each_asset_when_debugging()
        {
            // Arrange
            var renderer = this.CreateTagRenderer("/assets", new[] { "1" }, true, new Dictionary<string, IPackage>()
            {
                {"core", this.CreatePackage(new []{ "scripts/1.js", "scripts/2.js" })},
                {"ie", this.CreatePackage(new []{ "scripts/3.js", "scripts/4.js" })}
            });

            // Act
            var tag1 = renderer.JavaScriptTag("core", new object());
            var tag2 = renderer.JavaScriptTag("ie", new object());

            // Assert
            Assert.AreEqual("<script type=\"text/javascript\" src=\"/assets/core.js?debug=scripts/1.js\"></script>\r\n" +
                            "<script type=\"text/javascript\" src=\"/assets/core.js?debug=scripts/2.js\"></script>\r\n", tag1);
            Assert.AreEqual("<script type=\"text/javascript\" src=\"/assets/ie.js?debug=scripts/3.js\"></script>\r\n" +
                            "<script type=\"text/javascript\" src=\"/assets/ie.js?debug=scripts/4.js\"></script>\r\n", tag2);
        }

        [TestMethod]
        public void Can_render_javascript_tags_with_custom_attributes()
        {
            // Arrange
            var renderer = this.CreateTagRenderer("/assets", new[] { "1", "2" }, false, new Dictionary<string, IPackage>()
            {
                {"core", this.CreatePackage()}
            });

            // Act
            var ignoredAttr = renderer.JavaScriptTag("core", new { src="foo", type="fake" });
            var customAttr = renderer.JavaScriptTag("core", new { async = "true" });

            // Assert
            Assert.AreEqual("<script type=\"text/javascript\" src=\"/assets/core.js?v=1\"></script>\r\n", ignoredAttr);
            Assert.AreEqual("<script type=\"text/javascript\" src=\"/assets/core.js?v=2\" async=\"true\"></script>\r\n", customAttr);
        }

        [TestMethod]
        public void Can_render_stylesheet_tag_for_package_when_not_debugging()
        {
            // Arrange
            var renderer = this.CreateTagRenderer("/assets", new[] { "1", "2" }, false, new Dictionary<string, IPackage>()
            {
                {"core", this.CreatePackage()},
                {"ie", this.CreatePackage()}
            });

            // Act
            var tag1 = renderer.StyleSheetTag("core", new object());
            var tag2 = renderer.StyleSheetTag("ie", new object());

            // Assert
            Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"/assets/core.css?v=1\" />\r\n", tag1);
            Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"/assets/ie.css?v=2\" />\r\n", tag2);
        }

        [TestMethod]
        public void Can_render_stylesheet_tags_for_each_asset_when_debugging()
        {
            // Arrange
            var renderer = this.CreateTagRenderer("/assets", new[] { "1" }, true, new Dictionary<string, IPackage>()
            {
                {"core", this.CreatePackage(new []{ "style/1.css", "style/2.css" })},
                {"ie", this.CreatePackage(new []{ "style/3.css", "style/4.css" })}
            });

            // Act
            var tag1 = renderer.StyleSheetTag("core", new object());
            var tag2 = renderer.StyleSheetTag("ie", new object());

            // Assert
            Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"/assets/core.css?debug=style/1.css\" />\r\n" +
                            "<link rel=\"stylesheet\" type=\"text/css\" href=\"/assets/core.css?debug=style/2.css\" />\r\n", tag1);
            Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"/assets/ie.css?debug=style/3.css\" />\r\n" +
                            "<link rel=\"stylesheet\" type=\"text/css\" href=\"/assets/ie.css?debug=style/4.css\" />\r\n", tag2);
        }

        [TestMethod]
        public void Can_render_stylesheet_tags_with_custom_attributes()
        {
            // Arrange
            var renderer = this.CreateTagRenderer("/assets", new[] { "1", "2" }, false, new Dictionary<string, IPackage>()
            {
                {"core", this.CreatePackage()}
            });

            // Act
            var ignoredAttr = renderer.StyleSheetTag("core", new { rel = "foo", href = "foo", type = "fake" });
            var customAttr = renderer.StyleSheetTag("core", new { media = "print" });

            // Assert
            Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"/assets/core.css?v=1\" />\r\n", ignoredAttr);
            Assert.AreEqual("<link rel=\"stylesheet\" type=\"text/css\" href=\"/assets/core.css?v=2\" media=\"print\" />\r\n", customAttr);
        }

        private TagRenderer CreateTagRenderer(string routeRootPath, string[] versions, bool isDebugging, Dictionary<string, IPackage> packages)
        {
            var v = this.CreateVersionGenerator(versions);
            var d = this.CreateDebugState(isDebugging);

            return new TagRenderer(routeRootPath, packages, packages, v, d);
        }

        private IVersionGenerator CreateVersionGenerator(params string[] versions)
        {
            var versionGenerator = new Mock<IVersionGenerator>();
            versionGenerator.Setup(v => v.Generate()).ReturnsInOrder(versions);
            return versionGenerator.Object;
        }

        private IDebugState CreateDebugState(bool isDebugging)
        {
            var debugState = new Mock<IDebugState>();
            debugState.Setup(d => d.IsDebugging()).Returns(isDebugging);
            return debugState.Object;
        }

        private IPackage CreatePackage(string[] assetPaths)
        {
            var assets = new List<IAsset>();

            foreach(var path in assetPaths)
            {
                var asset = new Mock<IAsset>();
                asset.Setup(a => a.Path).Returns(path);
                assets.Add(asset.Object);
            }

            var package = new Mock<IPackage>();
            package.Setup(p => p.Assets).Returns(assets);
            return package.Object;
        }

        private IPackage CreatePackage()
        {
            var package = new Mock<IPackage>();
            return package.Object;
        }
    }
}
