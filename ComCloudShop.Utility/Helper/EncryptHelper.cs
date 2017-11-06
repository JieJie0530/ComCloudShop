using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.Utility
{
    public class EncryptHelper
    {
        private readonly static string DesKey = "YukiyamaYokizukeLisakoyoMisakisu";

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string MD5(string str, int length = 16)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            byte[] buffer2 = new MD5CryptoServiceProvider().ComputeHash(bytes);
            if (length == 16)
            {
                return BitConverter.ToString(buffer2).Replace("-", "").ToLower().Substring(8, 16);
            }
            return BitConverter.ToString(buffer2).Replace("-", "").ToLower();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SHA1(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            bytes = new SHA1CryptoServiceProvider().ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            foreach (byte num in bytes)
            {
                builder.AppendFormat("{0:x2}", num);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Encrypt3DESToBase64(string source)
        {
            try
            {
                var DES = new TripleDESCryptoServiceProvider();
                DES.Key = Convert.FromBase64String(DesKey);
                DES.Mode = CipherMode.ECB;
                DES.Padding = PaddingMode.PKCS7;
                ICryptoTransform DESEncrypt = DES.CreateEncryptor();
                byte[] Buffer = UTF8Encoding.UTF8.GetBytes(source);
                return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Decrypt3DESFromBase64(string source)
        {
            try
            {
                var DES = new TripleDESCryptoServiceProvider();
                DES.Key = Convert.FromBase64String(DesKey);
                DES.Mode = CipherMode.ECB;
                DES.Padding = PaddingMode.PKCS7;
                ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                byte[] Buffer = Convert.FromBase64String(source);
                return UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strString"></param>
        /// <returns>返回16进制字符形式</returns>
        public static string Encrypt3DESToHexString(string strString)
        {
            try
            {
                var DES = new TripleDESCryptoServiceProvider();
                DES.Key = Convert.FromBase64String(DesKey);
                DES.Mode = CipherMode.ECB;
                DES.Padding = PaddingMode.PKCS7;
                ICryptoTransform DESEncrypt = DES.CreateEncryptor();
                byte[] Buffer = UTF8Encoding.UTF8.GetBytes(strString);
                return ByteArrayToHexString(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        public static string Decrypt3DESFromHexString(string strString)
        {
            try
            {
                var DES = new TripleDESCryptoServiceProvider();
                DES.Key = Convert.FromBase64String(DesKey);
                DES.Mode = CipherMode.ECB;
                DES.Padding = PaddingMode.PKCS7;
                ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                byte[] Buffer = HexStringToByteArray(strString);
                return UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncodeBase64(string source)
        {
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(source);
            var result = string.Empty;
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string DecodeBase64(string source)
        {
            var result = string.Empty;
            try
            {
                byte[] bytes = Convert.FromBase64String(source);
                result = UTF8Encoding.UTF8.GetString(bytes);
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns>替换掉原有+和/符号</returns>
        public static string EncodeUrlSafeBase64(string source)
        {
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(source);
            var result = string.Empty;
            try
            {
                result = Convert.ToBase64String(bytes).Replace("/", "-").Replace("+", "$").Replace("=", "");
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="source"></param>
        /// <returns>替换掉改过的-和$符号</returns>
        public static string DecodeUrlSafeBase64(string source)
        {
            var result = string.Empty;
            source = source.Replace("-", "/").Replace("$", "+");
            var mod = source.Length % 4;
            var temp = "====";
            if (mod > 0)
            {
                source += temp.Substring(mod);
            }
            try
            {
                byte[] bytes = Convert.FromBase64String(source);
                result = UTF8Encoding.UTF8.GetString(bytes);
            }
            catch
            {

            }
            return result;
        }

        /// <summary>
        /// 字节数组转16进制
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        private static string ByteArrayToHexString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            var temp = hex.Replace("-", " ");
            //return temp;
            return HexStringToString(temp);
        }



        /// <summary>
        /// Hex字符串转换成字符串
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static string HexStringToString(string hex)
        {
            string[] hexValuesSplit = hex.Split(' ');
            var s = string.Empty;
            foreach (var h in hexValuesSplit)
            {
                var value = Convert.ToInt32(h, 16);
                //var stringValue = Char.ConvertFromUtf32(value);
                var charValue = (char)value;
                s += charValue;
            }
            return s;
        }
        /// <summary>
        /// 字符串转换成Hex字符串
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static string StringToHexString(string hex)
        {
            byte[] byteArray = Encoding.UTF32.GetBytes(hex);
            var g = BitConverter.ToString(byteArray);
            g = g.Replace("00-00-00", "").Replace("-", " ");
            return g;
        }


        /// <summary>
        /// 16进制转字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static byte[] HexStringToByteArray(string s)
        {
            s = StringToHexString(s);
            s = s.Replace(" ", "").Trim();
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }
            return buffer;
        }


    }
}
