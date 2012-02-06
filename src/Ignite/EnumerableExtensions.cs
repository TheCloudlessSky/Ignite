using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite
{
    internal static class EnumerableExtensions
    {
        public static byte[] Combine(this IList<byte[]> arrays)
        {
            var combined = new byte[arrays.Sum(b => b.Length)];
            int offset = 0;
            foreach (var array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, combined, offset, array.Length);
                offset += array.Length;
            }
            return combined;
        }
    }
}
