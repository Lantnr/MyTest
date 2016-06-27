using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TGG.Core.Common
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public class CryptoHelper
    {
        // 对称加密算法提供器
        /// <summary>加密器对象</summary>
        private ICryptoTransform encryptor;
        /// <summary>解密器对象</summary>
        private ICryptoTransform decryptor;
        /// <summary>Buffer大小</summary>
        private const int BufferSize = 1024;
        /// <summary>对称算法的初始化向量</summary>
        private Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        private string KEY = "A9s5D2Fr5Tg3H1Yw";

        /// <summary>构造函数</summary>
        /// <param name="algorithmName">要使用的System.Security.Cryptography.SymmetricAlgorithm 类的特定实现的名称</param>
        /// <param name="key">密钥(16位)</param>
        public CryptoHelper(string algorithmName, string key)
        {
            var provider = SymmetricAlgorithm.Create(algorithmName);
            if (String.IsNullOrEmpty(key)) key = KEY;
            provider.Key = Encoding.UTF8.GetBytes(key);
            provider.IV = IV;

            encryptor = provider.CreateEncryptor();
            decryptor = provider.CreateDecryptor();
        }

        /// <summary>构造函数</summary>
        /// <param name="key">密钥(16位)</param>
        public CryptoHelper(string key) : this("TripleDES", key) { }


        /// <summary>加密算法</summary>
        /// <param name="clearText">明文</param>
        /// <param name="key">密钥(16位)</param>
        /// <returns>加密文本</returns>
        public static string Encrypt(string clearText, string key)
        {
            var helper = new CryptoHelper(key);
            return helper.Encrypt(clearText);
        }

        /// <summary>解密算法</summary>
        /// <param name="encryptedText">加密文本</param>
        /// <param name="key">密钥密钥(16位)</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encryptedText, string key)
        {
            var helper = new CryptoHelper(key);
            return helper.Decrypt(encryptedText);
        }

        /// <summary>加密算法</summary>
        /// <param name="clearText">明文</param>
        /// <returns>加密文本</returns>
        public string Encrypt(string clearText)
        {
            // 创建明文流
            var clearBuffer = Encoding.UTF8.GetBytes(clearText);
            var clearStream = new MemoryStream(clearBuffer);

            // 创建空的密文流
            var encryptedStream = new MemoryStream();
            var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write);

            // 将明文流写入到buffer中
            // 将buffer中的数据写入到cryptoStream中
            var bytesRead = 0;
            var buffer = new byte[BufferSize];
            do
            {
                bytesRead = clearStream.Read(buffer, 0, BufferSize);
                cryptoStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            cryptoStream.FlushFinalBlock();

            // 获取加密后的文本
            buffer = encryptedStream.ToArray();
            var encryptedText = Convert.ToBase64String(buffer);
            return encryptedText;
        }

        /// <summary>解密算法</summary>
        /// <param name="encryptedText">加密文本</param>
        /// <returns>明文</returns>
        public string Decrypt(string encryptedText)
        {
            var encryptedBuffer = Convert.FromBase64String(encryptedText);
            Stream encryptedStream = new MemoryStream(encryptedBuffer);

            var clearStream = new MemoryStream();
            var cryptoStream = new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read);

            var bytesRead = 0;
            var buffer = new byte[BufferSize];
            do
            {
                bytesRead = cryptoStream.Read(buffer, 0, BufferSize);
                clearStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            buffer = clearStream.GetBuffer();
            var clearText = Encoding.UTF8.GetString(buffer, 0, (int)clearStream.Length);

            return clearText;
        }

    }
}