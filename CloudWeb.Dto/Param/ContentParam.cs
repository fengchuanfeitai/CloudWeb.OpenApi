using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CloudWeb.Dto.Param
{

    public class ContentParam : BaseContentParam
    {

    }

    /// <summary>
    /// 是否添加轮播
    /// </summary>
    public class TopStatusParam : BaseColumnParam
    {
        [Display(Name = "是否添加轮播状态")]
        [Required(ErrorMessage = "{0}必填")]
        public int TopStatus { get; set; }
    }

    /// <summary>
    /// 是否发布
    /// </summary>
    public class PublicStatusParam : BaseColumnParam
    {
        [Display(Name = "是否发布状态")]
        [Required(ErrorMessage = "{0}必填")]
        public int PublicStatus { get; set; }
    }

    /// <summary>
    /// 是否添加首页
    /// </summary>
    public class DefaultStatusParam : BaseColumnParam
    {
        [Display(Name = "添加首页状态")]
        [Required(ErrorMessage = "{0}必填")]
        public int DefaultStatus { get; set; }
    }

    public class BaseContentParam
    {
        [Display(Name = "主键")]
        [Required(ErrorMessage = "{0}必填")]
        public int Id { get; set; }
    }
}
