using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CloudWeb.Util
{
    /// <summary>
    /// md5辅助类
    /// </summary>
    public class MD5Util
    {
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return "";
                }
                var md5 = new MD5CryptoServiceProvider();
                string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str)), 4, 8).Replace("-", "");
                return t2;
            }
            catch (Exception)
            {
                return "";
            }

        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string str)
        {
            try
            {

                if (string.IsNullOrEmpty(str))
                {
                    return "";
                }
                MD5 md5 = MD5.Create(); //实例化一个md5对像
                                        // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                for (int i = 0; i < s.Length; i++)
                {
                    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                    str += s[i].ToString("X");
                }
                return str;
            }
            catch (Exception)
            {
                return "";
            }

        }

        /// <summary>
        /// 64位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return "";
                }
                //实例化一个md5对像
                MD5 md5 = MD5.Create();
                // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                return Convert.ToBase64String(s);
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
