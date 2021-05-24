﻿using System.Collections.Generic;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;

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
        ResponseResult<bool> DelCorporation(int[] ids);

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
        /// 分页查询公司信息
        /// </summary>
        /// <returns></returns>
        ResponseResult<IEnumerable<CorporationDto>> GetAllCorporation(BaseParam pageParam);
    }
}
