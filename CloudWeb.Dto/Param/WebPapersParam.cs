namespace CloudWeb.Dto.Param
{
    public class SearchPapers : BaseParam
    {
        /// <summary>
        /// 传入一级栏目Id
        /// </summary>
        public int MasterId { get; set; }
        /// <summary>
        /// 学科
        /// </summary>
        public int? ObjId { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
