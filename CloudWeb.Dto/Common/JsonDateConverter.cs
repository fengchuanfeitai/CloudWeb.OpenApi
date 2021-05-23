using Newtonsoft.Json.Converters;

namespace CloudWeb.Dto.Common
{
    /// <summary>
    /// 返回数据时间格式
    /// </summary>
    class JsonDateConverter : IsoDateTimeConverter
    {
        public JsonDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
