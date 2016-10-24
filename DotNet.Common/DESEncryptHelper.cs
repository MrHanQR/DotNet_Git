using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet.Common
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public class DESEncryptHelper
    {

        public DESEncryptHelper()
        {

        }

        private string key = "hqr"; //默认密钥

        private byte[] _sKey;
        private byte[] _sIV;

        #region 加密字符串

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为""(空)</param>
        /// <returns>输出加密后字符串</returns>
        public static string SEncryptString(string inputStr, string keyStr)
        {
            DESEncryptHelper ws = new DESEncryptHelper();
            return ws.EncryptString(inputStr, keyStr);
        }

        /// <summary>
        /// 加密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns>输出加密后字符串</returns>
        public static string SEncryptString(string inputStr)
        {
            DESEncryptHelper ws = new DESEncryptHelper();
            return ws.EncryptString(inputStr, "");
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns>输出加密后字符串</returns>
        private string EncryptString(string inputStr, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
            {
                keyStr = key;
            }
            byte[] inputByteArray = Encoding.Default.GetBytes(inputStr);
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            _sKey = new byte[8];
            _sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                _sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                _sIV[i - 8] = hb[i];
            des.Key = _sKey;
            des.IV = _sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            cs.Close();
            ms.Close();
            return ret.ToString();
        }
        #endregion

        #region 解密字符串
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        public static string SDecryptString(string inputStr, string keyStr)
        {
            DESEncryptHelper ws = new DESEncryptHelper();
            return ws.DecryptString(inputStr, keyStr);
        }
        /// <summary>
        ///  解密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <returns>解密后的结果</returns>
        public static string SDecryptString(string inputStr)
        {
            DESEncryptHelper ws = new DESEncryptHelper();
            return ws.DecryptString(inputStr, "");
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        private string DecryptString(string inputStr, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            byte[] inputByteArray = new byte[inputStr.Length / 2];
            for (int x = 0; x < inputStr.Length / 2; x++)
            {
                int i = (Convert.ToInt32(inputStr.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            _sKey = new byte[8];
            _sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                _sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                _sIV[i - 8] = hb[i];
            des.Key = _sKey;
            des.IV = _sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region 文件加密解密

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">加密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为""(空)</param>
        /// <returns></returns>  
        public static bool SEncryptFile(string filePath, string savePath, string keyStr)
        {
            DESEncryptHelper helper=new DESEncryptHelper();
            return helper.EncryptFile(filePath, savePath, keyStr);
        }
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">解密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>
        public static bool SDecryptFile(string filePath, string savePath, string keyStr)
        {
            DESEncryptHelper helper = new DESEncryptHelper();
            return helper.DecryptFile(filePath, savePath, keyStr);
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">加密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为""(空)</param>
        /// <returns></returns>  
        private bool EncryptFile(string filePath, string savePath, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            FileStream fs = File.OpenRead(filePath);
            byte[] inputByteArray = new byte[fs.Length];
            fs.Read(inputByteArray, 0, (int)fs.Length);
            fs.Close();
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            _sKey = new byte[8];
            _sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                _sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                _sIV[i - 8] = hb[i];
            des.Key = _sKey;
            des.IV = _sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);
            }
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">解密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>    
        private bool DecryptFile(string filePath, string savePath, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            FileStream fs = File.OpenRead(filePath);
            byte[] inputByteArray = new byte[fs.Length];
            fs.Read(inputByteArray, 0, (int)fs.Length);
            fs.Close();
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            _sKey = new byte[8];
            _sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                _sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                _sIV[i - 8] = hb[i];
            des.Key = _sKey;
            des.IV = _sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);
            }
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }
        #endregion

        #region Base64加密解密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            try
            {
                char[] Base64Code = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/', '=' };
                byte empty = (byte)0;
                ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
                StringBuilder outmessage;
                int messageLen = byteMessage.Count;
                int page = messageLen / 3;
                int use = 0;
                if ((use = messageLen % 3) > 0)
                {
                    for (int i = 0; i < 3 - use; i++)
                        byteMessage.Add(empty);
                    page++;
                }
                outmessage = new System.Text.StringBuilder(page * 4);
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    int[] outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    else
                        outstr[2] = 64;
                    if (!instr[2].Equals(empty))
                        outstr[3] = (instr[2] & 0x3f);
                    else
                        outstr[3] = 64;
                    outmessage.Append(Base64Code[outstr[0]]);
                    outmessage.Append(Base64Code[outstr[1]]);
                    outmessage.Append(Base64Code[outstr[2]]);
                    outmessage.Append(Base64Code[outstr[3]]);
                }
                return outmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            //将空格替换为加号
            text = text.Replace(" ", "+");

            try
            {
                if ((text.Length % 4) != 0)
                {
                    return "包含不正确的BASE64编码";
                }
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    return "包含不正确的BASE64编码";
                }
                string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int page = text.Length / 4;
                ArrayList outMessage = new ArrayList(page * 3);
                char[] message = text.ToCharArray();
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[4];
                    instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                    byte[] outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64)
                    {
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    if (instr[3] != 64)
                    {
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                        outMessage.Add(outstr[1]);
                    if (outstr[2] != 0)
                        outMessage.Add(outstr[2]);
                }
                byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                return Encoding.Default.GetString(outbyte);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region MD5运算
        /// <summary>
        /// 对字符串进行MD5运算,获得Hash描述
        /// </summary>
        /// <param name="str">要进行运算的字符串</param>
        /// <returns>string</returns>
        public static string GetStringMD5(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                MD5 md5 = MD5.Create();
                byte[] bytes = System.Text.Encoding.Default.GetBytes(str);
                byte[] md5Byte = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < md5Byte.Length; i++)
                {
                    sb.Append(md5Byte[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 对文件进行MD5运算,获得Hash描述
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>string</returns>
        public static string GetFileMd5(string path)
        {
            MD5 md5 = MD5.Create();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] md5byte = md5.ComputeHash(fs);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < md5byte.Length; i++)
                {
                    sb.Append(md5byte[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 对文件进行MD5运算,获得Hash描述
        /// </summary>
        /// <param name="data">文件流</param>
        /// <returns>string</returns>
        public static string GetFileMd5(byte[] data)
        {
            if (data.Length < 1)
            {
                return "";
            }
            else
            {
                //创建MD5密码服务提供程序
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                //计算传入的字节数组的哈希值
                byte[] result = md5.ComputeHash(data);

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                return Convert.ToBase64String(result);
            }
        }
        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static String GetStreamMd5(Stream stream)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;
            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            arrbytHashValue = oMD5Hasher.ComputeHash(stream); //计算指定Stream 对象的哈希值
            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            strHashData = System.BitConverter.ToString(arrbytHashValue);
            //替换-
            strHashData = strHashData.Replace("-", "");
            strResult = strHashData;
            return strResult;
        }
        #endregion

        #region RSA 加密解密

            #region RSA 的密钥产生

            /// <summary>
            /// RSA 的密钥产生 产生私钥 和公钥 
            /// </summary>
            /// <param name="xmlKeys">私钥</param>
            /// <param name="xmlPublicKey">公钥</param>
            public static void RSAKey(out string xmlKeys, out string xmlPublicKey)
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                xmlKeys = rsa.ToXmlString(true);
                xmlPublicKey = rsa.ToXmlString(false);
            }
            #endregion

            #region RSA的加密函数
            //############################################################################## 
            //RSA 方式加密 
            //说明KEY必须是XML的行式,返回的是字符串 
            //在有一点需要说明！！该加密方式有 长度 限制的！！ 
            //############################################################################## 

            /// <summary>
            /// RSA的加密函数
            /// RSA 方式加密 
            /// 说明KEY必须是XML的行式,返回的是字符串 
            /// 在有一点需要说明！！该加密方式有 长度 限制的！！ 
            /// </summary>
            /// <param name="xmlPublicKey">公钥</param>
            /// <param name="m_strEncryptString">要加密的字符串</param>
            /// <returns>string</returns>
            public static string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
            {

                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                PlainTextBArray = (new UnicodeEncoding()).GetBytes(m_strEncryptString);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;

            }
            /// <summary>
            /// RSA的加密函数
            /// RSA 方式加密 
            /// 说明KEY必须是XML的行式,返回的是字符串 
            /// 在有一点需要说明！！该加密方式有 长度 限制的！！ 
            /// </summary>
            /// <param name="xmlPublicKey">公钥</param>
            /// <param name="EncryptString">要加密的文件的文件流</param>
            /// <returns>string</returns>
            public static string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
            {

                byte[] CypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                CypherTextBArray = rsa.Encrypt(EncryptString, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;

            }
            #endregion

            #region RSA的解密函数
            /// <summary>
            /// RSA的解密函数
            /// </summary>
            /// <param name="xmlPrivateKey">私钥</param>
            /// <param name="m_strDecryptString">要进行解密的字符串</param>
            /// <returns>string</returns>
            public static string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
            {
                byte[] PlainTextBArray;
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                PlainTextBArray = Convert.FromBase64String(m_strDecryptString);
                DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return Result;

            }

            /// <summary>
            /// RSA的解密函数
            /// </summary>
            /// <param name="xmlPrivateKey">私钥</param>
            /// <param name="DecryptString">要进行解密的文件流</param>
            /// <returns>string</returns>
            public static string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
            {
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                DypherTextBArray = rsa.Decrypt(DecryptString, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return Result;

            }
            #endregion

        #endregion

        #region RSA数字签名

            #region RSA 私钥签名验证
            //RSA签名 
            /// <summary>
            /// RSA签名
            /// </summary>
            /// <param name="p_strKeyPrivate">私钥</param>
            /// <param name="HashbyteSignature">待签名的MD5Hash描述</param>
            /// <param name="EncryptedSignatureData">签名后的结果</param>
            /// <returns></returns>
            public static bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
            {

                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                return true;

            }

            //RSA签名 
            /// <summary>
            /// RSA签名
            /// </summary>
            /// <param name="p_strKeyPrivate">私钥</param>
            /// <param name="HashbyteSignature">待签名的MD5Hash描述</param>
            /// <param name="m_strEncryptedSignatureData">签名后的结果</param>
            /// <returns></returns>
            public static bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref string m_strEncryptedSignatureData)
            {

                byte[] EncryptedSignatureData;

                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

                return true;

            }

            //RSA签名 
            /// <summary>
            /// RSA签名
            /// </summary>
            /// <param name="p_strKeyPrivate">私钥</param>
            /// <param name="m_strHashbyteSignature">待签名的MD5Hash描述</param>
            /// <param name="EncryptedSignatureData">签名后的结果</param>
            /// <returns></returns>
            public static bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref byte[] EncryptedSignatureData)
            {

                byte[] HashbyteSignature;

                HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                return true;

            }

            //RSA签名 
            /// <summary>
            /// RSA签名 
            /// </summary>
            /// <param name="p_strKeyPrivate">私钥</param>
            /// <param name="m_strHashbyteSignature">待签名的MD5Hash描述</param>
            /// <param name="m_strEncryptedSignatureData">签名后的结果</param>
            /// <returns></returns>
            public static bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref string m_strEncryptedSignatureData)
            {

                byte[] HashbyteSignature;
                byte[] EncryptedSignatureData;

                HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

                return true;

            }
            #endregion

            #region RSA 公钥签名验证
            /// <summary>
            /// RSA签名
            /// </summary>
            /// <param name="p_strKeyPublic">公钥</param>
            /// <param name="HashbyteDeformatter">待签名MD5Hash描述</param>
            /// <param name="DeformatterData">签名后的结果</param>
            /// <returns></returns>
            public static bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
            {

                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5 
                RSADeformatter.SetHashAlgorithm("MD5");

                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            /// <summary>
            /// RSA签名
            /// </summary>
            /// <param name="p_strKeyPublic">公钥</param>
            /// <param name="p_strHashbyteDeformatter">待签名MD5Hash描述</param>
            /// <param name="DeformatterData">签名后的结果</param>
            /// <returns></returns>
            public static bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
            {

                byte[] HashbyteDeformatter;

                HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);

                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5 
                RSADeformatter.SetHashAlgorithm("MD5");

                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            /// <summary>
            /// RSA签名
            /// </summary>
            /// <param name="p_strKeyPublic">公钥</param>
            /// <param name="HashbyteDeformatter">待签名MD5Hash描述</param>
            /// <param name="p_strDeformatterData">签名后的结果</param>
            /// <returns></returns>
            public static bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
            {

                byte[] DeformatterData;

                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5 
                RSADeformatter.SetHashAlgorithm("MD5");

                DeformatterData = Convert.FromBase64String(p_strDeformatterData);

                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            /// <summary>
            /// RSA签名
            /// </summary>
            /// <param name="p_strKeyPublic">公钥</param>
            /// <param name="p_strHashbyteDeformatter">待签名MD5Hash描述</param>
            /// <param name="p_strDeformatterData">签名后的结果</param>
            /// <returns></returns>
            public static bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
            {

                byte[] DeformatterData;
                byte[] HashbyteDeformatter;

                HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5 
                RSADeformatter.SetHashAlgorithm("MD5");

                DeformatterData = Convert.FromBase64String(p_strDeformatterData);

                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }


            #endregion


        #endregion

    }
}