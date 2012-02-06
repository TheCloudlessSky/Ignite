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
    public class PackageBaseTest
    {
        [TestMethod]
        public void Can_get_data_from_all_assets()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Data("abc123").Build();
            var a2 = builder.Data("def456").Build();
            var a3 = builder.Data("ghi789").Build();

            var p = new TestablePackageBase("test", new[] { a1, a2, a3 });

            // Act
            var d = p.GetData();

            // Assert
            Assert.AreEqual("abc123\r\ndef456\r\nghi789\r\n", d);
        }

        [TestMethod]
        public void Can_get_data_from_individual_asset()
        {
            // Arrange
            var builder = new AssetBuilder();
            var a1 = builder.Path("content/1.js").Data("abc123").Build();
            var a2 = builder.Path("ContenT/2.js").Data("def456").Build();

            var p = new TestablePackageBase("test", new []{ a1, a2 });

            // Act/Assert
            Assert.AreEqual("abc123", p.GetData("content/1.js"));
            Assert.AreEqual("def456", p.GetData("cONTENt/2.js"));
            Assert.IsNull(p.GetData("non-existant-path/3.js"));
        }

        private class TestablePackageBase : PackageBase
        {
            public TestablePackageBase(string name, IList<IAsset> assets)
                : base(name, assets)
            {

            }

            protected override string Process(string data)
            {
                return data;
            }
        }

    }
}
