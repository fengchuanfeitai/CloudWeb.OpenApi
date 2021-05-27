namespace CloudWeb.Dto.Param
{

    public class ProductSearchParam : BaseParam
    {
        /// <summary>
        /// 所属公司
        /// </summary>
        public int? CorpId { get; set; }

        /// <summary>
        /// 产品关键字
        /// </summary>
        public string NameKeyword { get; set; }
    }

}
