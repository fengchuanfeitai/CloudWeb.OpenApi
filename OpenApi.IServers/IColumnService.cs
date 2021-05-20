﻿using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using System.Collections.Generic;

namespace CloudWeb.IServices
{
    public interface IColumnService : IBaseService
    {
        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<IEnumerable<ColumnDto>> GetAll();


        /// <summary>
        /// 查询栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<ColumnDto> GetColumn(int id);


        /// <summary>
        /// 修改栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> EdittColumn(ColumnDto columnDto);


        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> AddColumn(ColumnDto columnDto);


        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <returns></returns>
        ResponseResult<bool> DeleteColumn(dynamic[] ids);


    }
}