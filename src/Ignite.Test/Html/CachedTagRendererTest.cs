using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ignite.Html;
using Moq;

namespace Ignite.Test.Html
{
    [TestClass]
    public class CachedTagRendererTest
    {
        [TestMethod]
        public void Should_invalidate_javascript_tag_if_debugging()
        {
            // Arrange
            var renderer = this.CreateRenderer(true, new []{ "core", "ie" }, javascriptTags: new[] 
            { 
                new [] { "A1", "A2" }, new [] { "B1", "B2" },
            });

            // Act / Assert
            Assert.AreEqual("A1", renderer.JavaScriptTag("core", new { }));
            Assert.AreEqual("A2", renderer.JavaScriptTag("core", new { }));
            Assert.AreEqual("B1", renderer.JavaScriptTag("ie", new { }));
            Assert.AreEqual("B2", renderer.JavaScriptTag("ie", new { }));
        }

        [TestMethod]
        public void Should_invalidate_stylesheet_tag_if_debugging()
        {
            // Arrange
            var renderer = this.CreateRenderer(true, new[] { "core", "ie" }, stylesheetTags: new[] 
            { 
                new [] { "A1", "A2" }, new [] { "B1", "B2" },
            });

            // Act / Assert
            Assert.AreEqual("A1", renderer.StyleSheetTag("core", new { }));
            Assert.AreEqual("A2", renderer.StyleSheetTag("core", new { }));
            Assert.AreEqual("B1", renderer.StyleSheetTag("ie", new { }));
            Assert.AreEqual("B2", renderer.StyleSheetTag("ie", new { }));
        }

        [TestMethod]
        public void Should_cache_javascript_tag_if_not_debugging()
        {
            // Arrange
            var renderer = this.CreateRenderer(false, new[] { "core" }, javascriptTags: new[] 
            { 
                new [] { "A1", "A2", "A3", "A4" },
            });

            // Act / Assert
            Assert.AreEqual("A1", renderer.JavaScriptTag("core", new { }));
            Assert.AreEqual("A2", renderer.JavaScriptTag("core", new { foo = "bar" }));
            
            // Same attributes as before.
            Assert.AreEqual("A1", renderer.JavaScriptTag("core", new { }));
            Assert.AreEqual("A2", renderer.JavaScriptTag("core", new { foo = "bar" }));

            // new object() always generates a unique hash code.
            Assert.AreEqual("A3", renderer.JavaScriptTag("core", new object()));
            Assert.AreEqual("A4", renderer.JavaScriptTag("core", new object()));
        }

        [TestMethod]
        public void Should_cache_stylesheet_tag_if_not_debugging()
        {
            // Arrange
            var renderer = this.CreateRenderer(false, new[] { "core" }, stylesheetTags: new[] 
            { 
                new [] { "A1", "A2", "A3", "A4" },
            });

            // Act / Assert
            Assert.AreEqual("A1", renderer.StyleSheetTag("core", new { }));
            Assert.AreEqual("A2", renderer.StyleSheetTag("core", new { foo = "bar" }));

            // Same attributes as before.
            Assert.AreEqual("A1", renderer.StyleSheetTag("core", new { }));
            Assert.AreEqual("A2", renderer.StyleSheetTag("core", new { foo = "bar" }));

            // new object() always generates a unique hash code.
            Assert.AreEqual("A3", renderer.StyleSheetTag("core", new object()));
            Assert.AreEqual("A4", renderer.StyleSheetTag("core", new object()));
        }

        private CachedTagRenderer CreateRenderer(bool isDebugging, string[] packages, string[][] javascriptTags = null, string[][] stylesheetTags = null)
        {
            var ds = new Mock<IDebugState>();
            ds.Setup(d => d.IsDebugging()).Returns(isDebugging);

            var renderer = new Mock<ITagRenderer>();

            for (var i = 0; i < packages.Length; i++)
            {
                if (javascriptTags != null)
                {
                    renderer.Setup(r => r.JavaScriptTag(packages[i], It.IsAny<object>())).ReturnsInOrder(javascriptTags[i]);
                }

                if (stylesheetTags != null)
                {
                    renderer.Setup(r => r.StyleSheetTag(packages[i], It.IsAny<object>())).ReturnsInOrder(stylesheetTags[i]);
                }
            }

            return new CachedTagRenderer(renderer.Object, ds.Object);
        }
    }
}
