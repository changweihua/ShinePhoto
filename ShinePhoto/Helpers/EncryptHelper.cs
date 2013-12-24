using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ShinePhoto.Helpers
{
    /// <summary>
    /// MD5 加密帮助类
    /// </summary>
    public class MD5Helper
    {/// <summary>
        /// MD5 16位加密 加密后代码为大写
        /// </summary>
        /// <param name="cryptString">待加密的字符串</param>
        /// <returns>密文</returns>
        public static string GetMD5StringUpperCase(string cryptString)
        {
            string result = string.Empty;

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                result = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(cryptString)), 4, 8);
                result = result.Replace("-", "");
            }

            return result;
        }

        /// <summary>
        /// MD5 16位加密 加密后代码为小写
        /// </summary>
        /// <param name="cryptString">待加密的字符串</param>
        /// <returns>密文</returns>
        public static string GetMD5StringLowerCase(string cryptString)
        {
            return GetMD5StringUpperCase(cryptString).ToLower();
        }

        /// <summary>
        /// MD5 32位加密
        /// </summary>
        /// <param name="cryptString">明文</param>
        /// <returns>密文</returns>
        public static string GetMD5String(string cryptString)
        {
            string result = string.Empty;

            //实例化一个MD5对象
            using (MD5 md5 = MD5.Create())
            {
                StringBuilder sb = new StringBuilder();
                //加密后是一个字节类型的数组
                byte[] cryptStringArray = md5.ComputeHash(Encoding.UTF8.GetBytes(cryptString));
                //通过循环，将字节类型的数组转换成字符串
                for (int i = 0; i < cryptStringArray.Length; i++)
                {
                    //将得到的字符串使用16进制类型格式
                    sb.Append(cryptStringArray[i].ToString("X"));
                }

                result = sb.ToString();

            }

            return result;
        }


        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X").PadLeft(2, '0'));
            }

            return sb.ToString();
        }
    }
}
