using CloudWeb.Dto.Common;

namespace CloudWeb.Dto
{
    public class CorpProductsDto : BaseDto
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public int? Id { get; set; }

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
        /// 跳转链接
        /// </summary>
        public string LocationUrl { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int CorpId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string CorpName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public int IsShow { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }
    }
}
