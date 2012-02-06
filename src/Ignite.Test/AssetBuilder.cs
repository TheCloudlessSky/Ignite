using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ignite.Assets;
using Moq;

namespace Ignite.Test
{
    public class AssetBuilder
    {
        private string data;
        private string path;

        public AssetBuilder Data(string data)
        {
            this.data = data;
            return this;
        }

        public AssetBuilder Path(string path)
        {
            this.path = path;
            return this;
        }

        public IAsset Build()
        {
            var asset = new Mock<IAsset>();
            asset.Setup(a => a.GetData()).Returns(this.data);
            asset.Setup(a => a.Path).Returns(this.path);

            this.data = null;
            this.path = null;

            return asset.Object;
        }
    }
}
