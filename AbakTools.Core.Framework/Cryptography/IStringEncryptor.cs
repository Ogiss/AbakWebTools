using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Framework.Cryptography
{
    public interface IStringEncryptor
    {
        string EncryptString(string plainText);
        string DecryptString(string encryptedText);
    }
}
