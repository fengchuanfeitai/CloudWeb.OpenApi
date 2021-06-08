using CloudWeb.Dto.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudWeb.Util
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// 根据名称拿到枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static T GetEnum<T>(this string itemName)
        {
            return (T)Enum.Parse(typeof(T), itemName);
        }
        /// <summary>
        /// 根据枚举值拿到枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemValue"></param>
        /// <returns></returns>
        public static T GetEnum<T>(this int itemValue)
        {
            return (T)Enum.Parse(typeof(T), Enum.GetName(typeof(T), itemValue));
        }
        /// <summary>
        /// 根据枚举值拿到枚举名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemValue"></param>
        /// <returns></returns>
        public static string GetEnumName<T>(this int itemValue)
        {
            return Enum.GetName(typeof(T), itemValue);
        }
        /// <summary>
        /// 根据名称拿到枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static int GetEnumValue<T>(this string itemName)
        {
            return itemName.GetEnum<T>().GetHashCode();
        }
        /// <summary>
        /// 枚举获取描述
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum item)
        {
            Type type = item.GetType();
            MemberInfo[] memInfo = type.GetMember(item.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return item.ToString();//如果不存在描述，则返回枚举名称
        }

        public static List<SelectListItem> GetSelectListItem<T>(Enum language = null) where T : struct
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Array array = typeof(T).GetEnumValues();
            foreach (var t in array)
            {
                var type = t.GetType();//先获取这个枚举的类型
                var field = type.GetField(t.ToString());//通过这个类型获取到值
                var obj = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性

                var listItem = new SelectListItem()
                {
                    Value = t.GetHashCode(),
                    Text = obj.Name ?? t.ToString()
                };
                list.Add(listItem);
            }
            return list;
        }
    }
}