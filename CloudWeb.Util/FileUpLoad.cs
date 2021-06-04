using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CloudWeb.Util
{
    public class FileUpLoad
    {
        /// <summary>
        /// 判断是否是图片格式
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool IsImgFile(string extension)
        { //添加 json 文件路径
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            //创建配置根对象
            var configurationRoot = builder.Build();
            //appsetting.json中获取图片的配置
            string imageTypes = configurationRoot.GetSection("FileConfig:ImgExts").Value;
            if (imageTypes.Contains(extension))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否是视频格式
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool IsVideoFile(string extension)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            //创建配置根对象
            var configurationRoot = builder.Build();
            //appsetting.json中获取视频的配置
            string videoTypes = configurationRoot.GetSection("FileConfig:VideoExts").Value;
            if (videoTypes.Contains(extension))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建文件规则
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
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
