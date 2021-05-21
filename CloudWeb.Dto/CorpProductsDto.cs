using CloudWeb.Dto.Common;

namespace CloudWeb.Dto
{
    public class CorpProductsDto : BaseDto
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 产品内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int CorpId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsDisplay { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
    }
}
