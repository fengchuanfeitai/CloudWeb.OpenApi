using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudWeb.IServices
{
    public interface IContentService : IBaseService
    {
        /// <summary>
        /// 查询所有内容
        /// </summary>
        /// <returns></returns>
        ResponseResult<IEnumerable<ContentDto>> GetAll(BaseParam para);


        /// <summary>
        /// 查询内容
        /// </summary>
        /// <returns></returns>
        ResponseResult<ContentDto> GetContent(int id);


        /// <summary>
        /// 修改内容
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> EditContent(ContentDto contentDto);


        /// <summary>
        /// 添加内容
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> AddContent(ContentDto contentDto);


        /// <summary>
        /// 删除内容
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> DeleteContent(int[] ids);
    }
}
