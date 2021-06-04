namespace CloudWeb.Dto.Common
{
    /// <summary>
    /// 返回信息
    /// </summary>
    public class ContantMsg
    {
        #region 公用信息

        public const string msg = "选择删除的数据";

        public const string RequireMsg = "数据不能为空";

        public const string DeleteMsg = "请选者要删除的数据";

        #endregion

        #region 栏目返回信息

        /// <summary>
        /// 判断是否包含内容
        /// </summary>
        public const string DelColumn_ContainsContent_Msg = "当前栏目中包含内容数据，是否同时删除?";

        /// <summary>
        /// 删除栏目成功
        /// </summary>
        public const string DelColumn_Ok_Msg = "删除成功";

        /// <summary>
        /// 删除栏目失败
        /// </summary>
        public const string DelColumn_Fail_Msg = "删除成功";


        #endregion

        #region 内容返回信息
        /// <summary>
        /// 修改状态成功返回信息
        /// </summary>
        public const string ChangeStatus_OK_Msg = "修改状态成功";

        /// <summary>
        /// 修改状态信息失败返回信息
        /// </summary>
        public const string ChangeStatus_Fail_Msg = "修改状态失败";

        /// <summary>
        /// 删除未选择内容返回信息
        /// </summary>
        public const string DeleteContent_NoId_Msg = "修改状态失败";

        /// <summary>
        /// 编辑内容，未填写内容返回信息
        /// </summary>
        public const string EditContent_IsNull_Msg = "请输入内容信息";


        /// <summary>
        /// 编辑、添加内容时，栏目已经删除返回信息
        /// </summary>
        public const string AddContent_ColumnIsDel_Msg = "选择栏目已经删除，请重新添加";

        /// <summary>
        /// 编辑、添加内容时，栏目已经删除返回信息
        /// </summary>
        public const string EditContent_ColumnIsDel_Msg = "选择栏目已经删除，请重新编辑";
        #endregion
    }
}
