﻿using System.Collections.Generic;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;

namespace CloudWeb.IServices
{
    public interface ICorpProductsService : IBaseService
    {
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="corpProduct"></param>
        /// <returns></returns>
        ResponseResult<bool> AddProduct(CorpProductsDto corpProduct);

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ResponseResult<bool> DelProduct(dynamic[] ids);

        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="corpProduct"></param>
        /// <returns></returns>
        ResponseResult<bool> UpdateProduct(CorpProductsDto corpProduct);

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="id">产品id</param>
        /// <returns></returns>
        ResponseResult<CorpProductsDto> GetProductsById(int id);

        /// <summary>
        /// 根据公司Id获取产品
        /// </summary>
        /// <param name="corpId"></param>
        /// <returns></returns>
        ResponseResult<IEnumerable<CorpProductsDto>> GetProductsByCorpId(int corpId);

        /// <summary>
        /// 获取所有产品
        /// </summary>
        /// <returns></returns>
        ResponseResult<IEnumerable<CorpProductsDto>> GetProducts();
    }
}