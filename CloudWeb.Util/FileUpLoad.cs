using System;

namespace CloudWeb.Util
{
    public class FileUpLoad
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool IsImgFile(string extension)
        {
            string imageTypes = "|.jpg||.jpeg||.gif||.png||.bmp|";
            if (imageTypes.Contains(extension))
            {
                return true;
            }
            return false;
        }

        public static string CreateFileName(string extension)
        {
            Random ra = new Random(new Guid().GetHashCode());
            return string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssfff"), Next(10000, 99999), extension);
        }

        /// <summary>
        /// 生成指定范围的数字
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            Random random = new Random(GetRandomSeed());
            return random.Next(min, max);
        }

        /// <summary>
        /// 随机数种子，防止重复
        /// </summary>
        /// <returns></returns>
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

    }
}
