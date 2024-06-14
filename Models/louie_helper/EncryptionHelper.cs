using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PrjFunNowWeb.Models.louie_helper
{
    public static class EncryptionHelper
    {
        public static string Decrypt(string cipherText, string key)
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            using (var aes = Aes.Create())
            {
                var iv = new byte[aes.BlockSize / 8];
                var cipher = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                var keyBytes = Encoding.UTF8.GetBytes(key);
                using (var decryptor = aes.CreateDecryptor(keyBytes, iv))
                using (var ms = new MemoryStream(cipher))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
