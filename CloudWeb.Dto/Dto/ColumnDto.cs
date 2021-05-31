using CloudWeb.Dto.Common;

namespace CloudWeb.Dto
{
    public class ColumnDto : BaseDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 展示序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 是否是新闻
        /// </summary>
        public bool IsNews { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>

        public string ColName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        public string LocationUrl { get; set; }

        /// <summary>
        /// 封面网络地址
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// 内容模块上传封面描述
        /// </summary>
        public string ImgDesc1 { get; set; }

        /// <summary>
        /// 内容模块上传内页封面描述
        /// </summary>
        public string ImgDesc2 { get; set; }

        /// <summary>
        /// 图标网络地址
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 上传视频网络地址
        /// </summary>
        public string Video { get; set; }

        /// <summary>
        /// 栏目级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
    }

    public class ColumnDropDownDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>

        public string ColName { get; set; }

        /// <summary>
        /// 栏目级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 是否是新闻
        /// </summary>
        public bool IsNews { get; set; }
    }

}
