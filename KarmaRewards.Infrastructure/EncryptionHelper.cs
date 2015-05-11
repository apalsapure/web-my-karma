using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace KarmaRewards.Infrastructure
{
    public static class EncryptionHelper
    {
        #region Constants
        private const string ENCRYPT_KEY = "weVMJtWXUGlq2IL1HLiTt0p0g6gR8FOQidVJV/Db2rQ=";
        #endregion

        #region Public Methods
        public static StreamWriter GetEncryptor(Stream targetStream)
        {
            // Configure the desired encryption algorithm parameters
            SymmetricAlgorithm sa = new RijndaelManaged();
            sa.Key = Convert.FromBase64String(ENCRYPT_KEY);
            sa.IV = new byte[sa.BlockSize / 8];

            ICryptoTransform transform = sa.CreateEncryptor();
            return new StreamWriter(new CryptoStream(targetStream, transform, CryptoStreamMode.Write));
        }

        public static StreamReader GetDecryptor(Stream sourceStream)
        {
            // Configure the desired encryption algorithm parameters
            SymmetricAlgorithm sa = new RijndaelManaged();
            sa.Key = Convert.FromBase64String(ENCRYPT_KEY);
            sa.IV = new byte[sa.BlockSize / 8];

            ICryptoTransform transform = sa.CreateDecryptor();
            return new StreamReader(new CryptoStream(sourceStream, transform, CryptoStreamMode.Read));
        }

        public static string Encrypt<T>(T obj)
        {
            // Configure the desired encryption algorithm parameters
            SymmetricAlgorithm sa = new RijndaelManaged();
            sa.Key = Convert.FromBase64String(ENCRYPT_KEY);
            sa.IV = new byte[sa.BlockSize / 8];

            ICryptoTransform transform = sa.CreateEncryptor();
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (MemoryStream outputStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(outputStream, transform, CryptoStreamMode.Write))
            {
                serializer.Serialize(cryptoStream, obj);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }

        public static T Decrypt<T>(string data)
        {
            // Configure the desired encryption algorithm parameters
            SymmetricAlgorithm sa = new RijndaelManaged();
            sa.Key = Convert.FromBase64String(ENCRYPT_KEY);
            sa.IV = new byte[sa.BlockSize / 8];

            ICryptoTransform transform = sa.CreateDecryptor();
            XmlSerializer deSerializer = new XmlSerializer(typeof(T));

            using (MemoryStream inputStream = new MemoryStream(Convert.FromBase64String(data)))
            {
                using (Stream cryptoStream = new CryptoStream(inputStream, transform, CryptoStreamMode.Read))
                {
                    return (T)deSerializer.Deserialize(cryptoStream);
                }
            }
        }
        #endregion
    }
}
