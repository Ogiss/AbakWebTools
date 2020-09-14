using System;
using System.Security.Cryptography;
using System.Text;

namespace AbakTools.Core.Framework.Cryptography
{
    public static class HashTools
    {
        public static bool SHA1HashEquals(byte[] a, byte[] b)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                var hashA = GetHash(sha1, a);
                var hashB = GetHash(sha1, b);

                return StringComparer.OrdinalIgnoreCase.Compare(hashA, hashB) == 0;
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorytm, byte[] data)
        {
            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
