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
    public class AssetTest
    {
        [TestMethod]
        public void Can_get_data_from_file_system()
        {
            var fs = new Mock<IFileSystem>();
            fs.Setup(f => f.ReadFile("C:\\app\\content\\script1.js")).Returns("abc123");
            var a = new Asset("content/script1.js", "C:\\app\\content\\script1.js", fs.Object);
            Assert.AreEqual("abc123", a.GetData());
        }
    }
}
