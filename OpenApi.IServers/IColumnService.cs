using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Dto;
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
        ResponseResult<ColumnSelectDto> GetColumn(int id);

        /// <summary>
        /// 根据父Id获取子栏目
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        ResponseResult<IEnumerable<ColumnDto>> GetColumnsByParent(int parentId, int? level);


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
        /// 栏目下级是否包含数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ResponseResult<bool> IsContainsContent(int[] ids);

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> DeleteColumn(int[] ids);

        /// <summary>
        /// 下拉框数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        ResponseResult<IList<ColumnDropDownDto>> GetDropDownList(int? id, bool existTopLevel);


        ResponseResult<IList<SelectListItem>> GetModuleDownList();

        #endregion

        #region 网站接口

        /// <summary>
        /// 获取导航栏数据
        /// </summary>
        /// <returns></returns>
        ResponseResult<IEnumerable<ColumnDto>> GetColumnsByParentId(int parentId);

        /// <summary>
        /// 获取栏目轮播图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResponseResult<IList<CarouselDto>> GetCarouselImg(int columnId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        ResponseResult<IEnumerable<ColumnDto>> GetExperimentCol(int columnId, int level);

        #endregion
    }
}
