using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using System.Collections.Generic;

namespace CloudWeb.IServices
{
    public interface IContentService : IBaseService
    {
        #region 后台接口

        /// <summary>
        /// 查询所有内容
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ResponseResult<IEnumerable<ContentDto>> GetAll(SearchParam para);

        /// <summary>
        /// 改变添加轮播
        /// </summary>
        /// <param name="TopStatusParam">状态参数</param>
        /// <returns></returns>
        ResponseResult ChangeTopStatus(TopStatusParam showStatusParam);

        /// <summary>
        /// 改变发布状态
        /// </summary>
        /// <param name="PublicStatusParam">状态参数</param>
        /// <returns></returns>
        ResponseResult ChangePublicStatus(PublicStatusParam showStatusParam);

        /// <summary>
        /// 改变添加首页
        /// </summary>
        /// <param name="DefaultStatusParam">状态参数</param>
        /// <returns></returns>
        ResponseResult ChangeDefaultStatus(DefaultStatusParam showStatusParam);

        /// <summary>
        /// 查询内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResponseResult<ContentDto> GetContent(int id);

        /// <summary>
        /// 修改内容
        /// </summary>
        /// <param name="contentDto"></param>
        /// <returns></returns>
        ResponseResult<bool> EditContent(ContentDto contentDto);

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="contentDto"></param>
        /// <returns></returns>
        ResponseResult<bool> AddContent(ContentParam contentParam);    

        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ResponseResult<bool> DeleteContent(int[] ids);
        #endregion

        #region 网站接口

        /// <summary>
        /// 获取首页新闻
        /// </summary>
        /// <param name="isCarousel">是否未轮播图</param>
        /// <returns></returns>
        ResponseResult<IEnumerable<ContentDto>> GetIndexNews(bool isCarousel);

       

        /// <summary>
        /// 根据columnId查询内容
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        ResponseResult<IEnumerable<ContentDto>> GetContentByColumnId(SearchPapers param);
        #endregion
    }
}
