using System;
using System.Collections.Generic;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;

namespace CloudWeb.IServices
{
    public interface ICorporationService : IBaseService
    {
        /// <summary>
        /// 添加公司信息
        /// </summary>
        /// <param name="corporation"></param>
        /// <returns></returns>
        ResponseResult<bool> AddCorporation(CorporationDto corporation);

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ResponseResult<bool> DelCorporation(dynamic[] ids);

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="id"></param>    
        /// <returns></returns>
        ResponseResult<bool> UpdateCorporation(CorporationDto corporation);

        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResponseResult<CorporationDto> GetCorporation(int id);

        
        /// <summary>
        /// 获取全部的公司信息
        /// </summary>
        /// <returns></returns>
        ResponseResult<IEnumerable<CorporationDto>> GetCorporation();
    }
}
