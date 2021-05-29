using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CloudWeb.Dto.Param
{

    public class ContentParam
    {

        /// <summary>
        /// 所属栏目id
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 浏览
        /// </summary>
        public int Hits { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string ImgUrl1 { get; set; }

        /// <summary>
        /// 新闻内页封面
        /// </summary>
        public string ImgUrl2 { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 是否添加到轮播
        /// </summary>
        public bool IsCarousel { get; set; }

        /// <summary>
        /// 是否推荐到首页
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataType(DataType.Date)]
        //[JsonConverter(typeof(JsonDateConverter))]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataType(DataType.Date)]
        //[JsonConverter(typeof(JsonDateConverter))]
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 创建者id
        /// </summary>
        public int Creator { get; set; }

        /// <summary>
        /// 修改者id
        /// </summary>
        public int Modifier { get; set; }
    }

    /// <summary>
    /// 是否添加轮播
    /// </summary>
    public class CarouselStatusParam : BaseColumnParam
    {
        [Display(Name = "是否添加轮播状态")]
        [Required(ErrorMessage = "{0}必填")]
        public int CarouselStatus { get; set; }
    }

    /// <summary>
    /// 是否发布
    /// </summary>
    public class PublicStatusParam : BaseColumnParam
    {
        [Display(Name = "是否发布状态")]
        [Required(ErrorMessage = "{0}必填")]
        public bool PublicStatus { get; set; }
    }

    /// <summary>
    /// 是否添加首页
    /// </summary>
    public class DefaultStatusParam : BaseColumnParam
    {
        [Display(Name = "添加首页状态")]
        [Required(ErrorMessage = "{0}必填")]
        public bool DefaultStatus { get; set; }
    }

    public class BaseContentParam
    {
        [Required(ErrorMessage = "主键必填")]
        public int Id { get; set; }
    }

    public class SearchParam : BaseParam
    {
        /// <summary>
        /// 所属栏目id
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 标题关键字
        /// </summary>
        public string TitleKeyword { get; set; }
    }
}
