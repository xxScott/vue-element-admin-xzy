using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Utilities
{
    public class Encrypt
    {
        ~Encrypt()
        {
            if (m_InstanceCount > 0) m_InstanceCount--;
        }

        private static int m_InstanceCount = 0;
        private static Encrypt m_Instance = null;
        public static Encrypt Instance
        {
            get
            {
                if (m_InstanceCount == 0)
                {
                    m_Instance = new Encrypt();
                    m_InstanceCount++;
                }
                return m_Instance;
            }
        }



        public string key = "1234567890"; //默认密钥 

        private byte[] skey;
        private byte[] siv;

        public string EncryptString(string inputstr)
        {
            return this.EncryptString(inputstr, key);
        }

        /// <summary> 
        /// 加密字符串 
        /// </summary> 
        /// <param name="inputstr">输入字符串</param> 
        /// <param name="keystr">密码，可以为“”</param> 
        /// <returns>输出加密后字符串</returns> 
        public string EncryptString(string inputstr, string keystr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keystr == "")
                keystr = key;
            byte[] inputbytearray = Encoding.Default.GetBytes(inputstr);
            byte[] keybytearray = Encoding.Default.GetBytes(keystr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keybytearray);
            skey = new byte[8];
            siv = new byte[8];
            for (int i = 0; i < 8; i++)
                skey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                siv[i - 8] = hb[i];
            des.Key = skey;
            des.IV = siv;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputbytearray, 0, inputbytearray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:x2}", b);
            }
            cs.Close();
            ms.Close();
            return ret.ToString();
        }

        /// <summary> 
        /// 加密文件 
        /// </summary> 
        /// <param name="filepath">输入文件路径</param> 
        /// <param name="savepath">加密后输出文件路径</param> 
        /// <param name="keystr">密码，可以为“”</param> 
        /// <returns></returns> 
        public bool EncryptFile(string filepath, string savepath, string keystr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keystr == "")
                keystr = key;
            FileStream fs = File.OpenRead(filepath);
            byte[] inputbytearray = new byte[fs.Length];
            fs.Read(inputbytearray, 0, (int)fs.Length);
            fs.Close();
            byte[] keybytearray = Encoding.Default.GetBytes(keystr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keybytearray);
            skey = new byte[8];
            siv = new byte[8];
            for (int i = 0; i < 8; i++)
                skey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                siv[i - 8] = hb[i];
            des.Key = skey;
            des.IV = siv;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputbytearray, 0, inputbytearray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savepath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);
            }
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }

        public string DecryptString(string inputstr)
        {
            return this.DecryptString(inputstr, key);
        }

        /// <summary> 
        /// 解密字符串 
        /// </summary> 
        /// <param name="inputstr">要解密的字符串</param> 
        /// <param name="keystr">密钥</param> 
        /// <returns>解密后的结果</returns> 
        public string DecryptString(string inputstr, string keystr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keystr == "")
                keystr = key;
            byte[] inputbytearray = new byte[inputstr.Length / 2];
            for (int x = 0; x < inputstr.Length / 2; x++)
            {
                int i = (Convert.ToInt32(inputstr.Substring(x * 2, 2), 16));
                inputbytearray[x] = (byte)i;
            }
            byte[] keybytearray = Encoding.Default.GetBytes(keystr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keybytearray);
            skey = new byte[8];
            siv = new byte[8];
            for (int i = 0; i < 8; i++)
                skey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                siv[i - 8] = hb[i];
            des.Key = skey;
            des.IV = siv;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputbytearray, 0, inputbytearray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary> 
        /// 解密文件 
        /// </summary> 
        /// <param name="filepath">输入文件路径</param> 
        /// <param name="savepath">解密后输出文件路径</param> 
        /// <param name="keystr">密码，可以为“”</param> 
        /// <returns></returns> 
        public bool DecryptFile(string filepath, string savepath, string keystr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keystr == "")
                keystr = key;
            FileStream fs = File.OpenRead(filepath);
            byte[] inputbytearray = new byte[fs.Length];
            fs.Read(inputbytearray, 0, (int)fs.Length);
            fs.Close();
            byte[] keybytearray = Encoding.Default.GetBytes(keystr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keybytearray);
            skey = new byte[8];
            siv = new byte[8];
            for (int i = 0; i < 8; i++)
                skey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                siv[i - 8] = hb[i];
            des.Key = skey;
            des.IV = siv;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputbytearray, 0, inputbytearray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savepath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);
            }
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }
    }
}
