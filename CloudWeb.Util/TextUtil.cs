using System;
using System.Text.RegularExpressions;

namespace CloudWeb.Util
{
    public static class TextUtil
    {
        /// <summary>
        /// 截取文本
        /// </summary>
        /// <param name="txt">文本</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string StringTruncat(string text, int maxLength, string endWith)
        {
            //判断原字符串是否为空
            if (string.IsNullOrEmpty(text))
                return text + endWith;

            //返回字符串的长度必须大于1
            if (maxLength < 1)
                throw new Exception("返回的字符串长度必须大于[0] ");

            //判断原字符串是否大于最大长度
            if (text.Length > maxLength)
            {
                //截取原字符串
                string strTmp = text.Substring(0, maxLength);

                //判断后缀是否为空
                if (string.IsNullOrEmpty(endWith))
                    return strTmp;
                else
                    return strTmp + endWith;
            }
            return text;
        }

        /// <summary>
        /// 过滤Html标签除特定标签
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string FilterHtml(string txt)
        {
            //string regexstr = @"<[^>]*>";    //去除所有的标签
            //@"<script[^>]*?>.*?</script>" //去除所有脚本，中间部分也删除
            // string regexstr = @"<img[^>]*>";   //去除图片的正则
            // string regexstr = @"<(?!br).*?>";   //去除所有标签，只剩br
            // string regexstr = @"<table[^>]*?>.*?</table>";   //去除table里面的所有内容
            //string regexstr = @"<(?!img|br|p|/p).*?>";   //去除所有标签，只剩img,br,p

            string regexstr = @"<[^>]*>";   //去除所有标签，只剩img,a
            txt = Regex.Replace(txt, regexstr, string.Empty, RegexOptions.IgnoreCase);

            return txt;

        }
    }
}
