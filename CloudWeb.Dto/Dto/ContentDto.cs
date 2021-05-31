using CloudWeb.Dto.Common;
using System;

namespace CloudWeb.Dto
{
    /// <summary>
    /// 内容数据
    /// </summary>
    public class ContentDto : BaseDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 所属栏目id
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 所属栏目id
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 浏览
        /// </summary>
        public int? Hits { get; set; }

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
    }

    public class ContentSelectDto
    {/// <summary>
     /// 编号
     /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 所属栏目id
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 所属栏目id
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 浏览
        /// </summary>
        public int? Hits { get; set; }

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
    }
}
