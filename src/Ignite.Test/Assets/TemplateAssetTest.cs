using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ignite.Assets;

namespace Ignite.Test.Assets
{
    [TestClass]
    public class TemplateAssetTest
    {
        [TestMethod]
        public void Should_add_templates_to_keyed_by_stripping_the_file_extension()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("tmpl/A.jst").Data("<h1 />").Build();
            var a2 = builder.Path("views/B.jst").Data("<h2 />").Build();

            var config = new TemplateConfiguration("JST", "tmpl", "jst");
            var tmpl = this.CreateTemplateAsset(config, a1, a2);

            // Act
            var data = tmpl.GetData();

            // Assert
            Assert.IsTrue(data.Contains("ns[\"tmpl/A\"] = lazyTemplate('<h1 />');"));
            Assert.IsTrue(data.Contains("ns[\"views/B\"] = lazyTemplate('<h2 />');"));
        }

        [TestMethod]
        public void Should_escape_single_quotes_for_asset_data()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("a.jst").Data("<a href='#' />").Build();
            var a2 = builder.Path("b.jst").Data("<a href=\\'#\\' />").Build();
            var a3 = builder.Path("c.jst").Data("<a href=\"#\" />").Build();

            var config = new TemplateConfiguration("JST", "tmpl", "jst");
            var tmpl = this.CreateTemplateAsset(config, a1, a2, a3);

            // Act
            var data = tmpl.GetData();

            // Assert
            Assert.IsTrue(data.Contains("ns[\"a\"] = lazyTemplate('<a href=\\'#\\' />');"));
            Assert.IsTrue(data.Contains("ns[\"b\"] = lazyTemplate('<a href=\\\\'#\\\\' />');"));
            Assert.IsTrue(data.Contains("ns[\"c\"] = lazyTemplate('<a href=\"#\" />');"));
        }

        [TestMethod]
        public void Should_replace_line_breaks_with_new_line_characters()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("a.jst").Data(@"<h1>
</h1>").Build();
            var a2 = builder.Path("b.jst").Data(@"<h1>
    </h1>").Build();

            var config = new TemplateConfiguration("JST", "tmpl", "jst");
            var tmpl = this.CreateTemplateAsset(config, a1, a2);

            // Act
            var data = tmpl.GetData();

            // Assert
            Assert.IsTrue(data.Contains("ns[\"a\"] = lazyTemplate('<h1>\\n</h1>');"));
            // Preserve whitespace.
            Assert.IsTrue(data.Contains("ns[\"b\"] = lazyTemplate('<h1>\\n    </h1>');"));
        }

        [TestMethod]
        public void Can_use_lower_case_names()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("tMpL/A.jst").Data("<h1 />").Build();
            var a2 = builder.Path("VIEWS/b.jst").Data("<h2 />").Build();

            var config = new TemplateConfiguration("JST", "tmpl", "jst");
            config.UseLowerCaseNames = true;
            var tmpl = this.CreateTemplateAsset(config, a1, a2);

            // Act
            var data = tmpl.GetData();

            // Assert
            Assert.IsTrue(data.Contains("ns[\"tmpl/a\"] = lazyTemplate('<h1 />');"));
            Assert.IsTrue(data.Contains("ns[\"views/b\"] = lazyTemplate('<h2 />');"));
        }

        [TestMethod]
        public void Should_remove_common_prefixes()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("tmpl/a.jst").Data("<h1 />").Build();
            var a2 = builder.Path("tmpl/b.jst").Data("<h1 />").Build();
            var a3 = builder.Path("tmpl/c.jst").Data("<h1 />").Build();
            var a4 = builder.Path("tmpl/test/d.jst").Data("<h1 />").Build();

            var config = new TemplateConfiguration("JST", "tmpl", "jst");
            var tmpl = this.CreateTemplateAsset(config, a1, a2, a3, a4);

            // Act
            var data = tmpl.GetData();

            // Assert
            Assert.IsTrue(data.Contains("ns[\"a\"] = lazyTemplate('<h1 />');"));
            Assert.IsTrue(data.Contains("ns[\"b\"] = lazyTemplate('<h1 />');"));
            Assert.IsTrue(data.Contains("ns[\"c\"] = lazyTemplate('<h1 />');"));
            Assert.IsTrue(data.Contains("ns[\"test/d\"] = lazyTemplate('<h1 />');"));
        }

        private TemplateAsset CreateTemplateAsset(TemplateConfiguration tmplConfiguration, params IAsset[] assets)
        {
            return new TemplateAsset(Guid.NewGuid().ToString(), assets, tmplConfiguration);
        }
    }
}
