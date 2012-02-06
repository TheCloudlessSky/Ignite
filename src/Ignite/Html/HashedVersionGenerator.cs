using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Ignite.Html
{
    public class HashedVersionGenerator : IVersionGenerator
    {
        public string Generate()
        {
            var bytes = new byte[6];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
