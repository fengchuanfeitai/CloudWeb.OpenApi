using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudWeb.IServices
{
    public interface IContentService : IBaseService
    {
        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<IEnumerable<ContentDto>> GetAll();


        /// <summary>
        /// 查询栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<ContentDto> GetContent(int id);


        /// <summary>
        /// 修改栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> EdittContent(ContentDto contentDto);


        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> AddContent(ContentDto contentDto);


        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> DeleteContent(dynamic[] ids);
    }
}
