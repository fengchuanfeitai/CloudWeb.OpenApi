using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using CloudWeb.OpenApi.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudWeb.Services
{
    public class ColumnService : BaseDao<ColumnDto>, IColumnService
    {
        const string sql = @"SELECT [ColumnId],[CreateTime],[ModifyTime],[Creator],[Modifier],[ColName],[Level],[Summary],[LocationUrl],[Cover],[ImgDesc1],[ImgDesc2],[Icon],[Video],[ParentId],[Sort],[IsNews] ,[IsShow] ,[IsDel]FROM[Ori_CloudWeb].[dbo].[Columns] ORDER BY [CreateTime] DESC ";
        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <returns></returns>
        public ResponseResult<IEnumerable<ColumnDto>> GetAll()
        {
            ResponseResult<IEnumerable<ColumnDto>> result = new ResponseResult<IEnumerable<ColumnDto>>();
            return result.SetData(GetAll(sql));
        }
    }
}
