using System.ComponentModel.DataAnnotations;

namespace CloudWeb.Dto.Param
{
    /// <summary>
    /// 栏目查询参数
    /// </summary>
    public class ColumnParam : BaseParam
    {
        [Display(Name = "栏目名称")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(16, ErrorMessage = "不能超过{0}个字符")]
        public string ColumnName { get; set; }
    }

    public class ColumnEditParam
    {



    }

    /// <summary>
    /// 改变状态
    /// </summary>
    public class ShowStatusParam : BaseColumnParam
    {
        [Display(Name = "显示状态")]
        [Required(ErrorMessage = "{0}必填")]
        public int ShowStatus { get; set; }

    }

    /// <summary>
    /// 基础参数
    /// </summary>
    public class BaseColumnParam
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
    }
}
