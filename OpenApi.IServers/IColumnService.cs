using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using System.Collections.Generic;

namespace CloudWeb.IServices
{
    public interface IColumnService : IBaseService
    {
        #region 后台接口


        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<IEnumerable<ColumnDto>> GetAll(BaseParam pageParam);

        /// <summary>
        /// 改变显示状态
        /// </summary>
        /// <param name="showStatusParam">状态参数</param>
        /// <returns></returns>
        ResponseResult ChangeShowStatus(ShowStatusParam showStatusParam);

        /// <summary>
        /// 查询栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<ColumnDto> GetColumn(int id);

        /// <summary>
        /// 根据父Id获取子栏目
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        ResponseResult<IEnumerable<ColumnDto>> GetColumnsByParent(int parentId);


        /// <summary>
        /// 修改栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> EditColumn(ColumnDto columnDto);


        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> AddColumn(ColumnDto columnDto);


        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> DeleteColumn(int[] ids);

        #endregion

        #region 网站接口

        /// <summary>
        /// 获取导航栏数据
        /// </summary>
        /// <returns></returns>
        ResponseResult GetMenus();

        /// <summary>
        /// 获取图标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResponseResult GetIcons(int id);




        #endregion
    }
}
