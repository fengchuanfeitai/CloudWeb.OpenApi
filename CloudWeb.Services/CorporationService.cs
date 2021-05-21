﻿using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using System;
using System.Collections.Generic;

namespace CloudWeb.Services
{
    public class CorporationService : BaseDao<CorporationDto>, ICorporationService
    {
        /// <summary>
        /// 添加公司信息
        /// </summary>
        /// <param name="corporation"></param>
        /// <returns></returns>
        public ResponseResult<bool> AddCorporation(CorporationDto corporation)
        {
            const string InsertSql = @"INSERT INTO dbo.Corporations
               (CreateTime,ModifyTime,Creator,Modifier,[Name],Cover,Logo1,Logo2,ColumnId,
                AboutUs,AboutUsCover,ContactUs,ContactUsBg,Sort,IsDisplay,IsDel)
                VALUES
                (@CreateTime,@ModifyTime,@Creator,@Modifier,@Name,@Cover,@Logo1,@Logo2,@ColumnId,
                 @AboutUs,@AboutUsCover,@ContactUs,@ContactUsBg,@Sort,@IsDisplay,@IsDel)";

            return new ResponseResult<bool>(Add(InsertSql, corporation));
        }

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ResponseResult<bool> DelCorporation(dynamic[] ids)
        {
            string DelSql = "UPDATE dbo.Corporations SET IsDel = 1 WHERE CorpId= @ids";
            if (ids.Length > 1)
                DelSql = "UPDATE dbo.Corporations SET IsDel = 1 WHERE CorpId in @ids";

            return new ResponseResult<bool>(Update(DelSql, ids));
        }

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="corporation"></param>
        /// <returns></returns>
        public ResponseResult<bool> UpdateCorporation(CorporationDto corporation)
        {
            var Corporation = GetCorporation(corporation.CorpId);
            if (Corporation == null)
                return new ResponseResult<bool>(false, "修改失败，公司信息不存在。");

            if (corporation.Name == Corporation.Result.Name &&
                corporation.Cover == Corporation.Result.Cover &&
                corporation.Logo1 == Corporation.Result.Logo1 &&
                corporation.Logo2 == Corporation.Result.Logo2 &&
                corporation.ColumnId == Corporation.Result.ColumnId &&
                corporation.AboutUs == Corporation.Result.AboutUs &&
                corporation.AboutUsCover == Corporation.Result.AboutUsCover &&
                corporation.ContactUs == Corporation.Result.ContactUs &&
                corporation.ContactUsBg == Corporation.Result.ContactUsBg &&
                corporation.sort == Corporation.Result.sort &&
                corporation.IsDisplay == Corporation.Result.IsDisplay &&
                corporation.IsDel == Corporation.Result.IsDel)
            {
                return new ResponseResult<bool>(true, "修改成功");
            }

            const string UpdateSql = @"UPDATE dbo.Corporations SET ModifyTime=@ModifyTime,Modifier=@Modifier,
                    [Name]=@Name,Cover=@Cover,Logo1=@Logo1, Logo2=@Logo2,ColumnId=@ColumnId,AboutUs=@AboutUs,
                    AboutUsCover=@AboutUsCover,ContactUs=@ContactUs,ContactUsBg=@ContactUsBg,Sort=@Sort,
                    IsDisplay=@IsDisplay,IsDel=@IsDel
                    WHERE IsDel=0 AND CorpId=@CorpId";

            return new ResponseResult<bool>(Update(UpdateSql, corporation));
        }

        public ResponseResult<CorporationDto> GetCorporation(int id)
        {
            const string SelSql = @"SELECT CorpId,CreateTime,ModifyTime,Creator,Modifier,
                [Name],Cover,Logo1,Logo2,ColumnId,AboutUs,AboutUsCover,ContactUs,ContactUsBg,
                Sort,IsDisplay,IsDel FROM dbo.Corporations
                WHERE IsDel=0 AND CorpId=@id ORDER BY CreateTime DESC";

            return new ResponseResult<CorporationDto>(Find(SelSql, id));
        }

        public ResponseResult<IEnumerable<CorporationDto>> GetAllCorporation()
        {
            const string SelSql = @"SELECT CorpId,CreateTime,ModifyTime,Creator,Modifier,
                [Name],Cover,Logo1,Logo2,ColumnId,AboutUs,AboutUsCover,ContactUs,ContactUsBg,
                Sort,IsDisplay,IsDel FROM dbo.Corporations 
                WHERE IsDel=0 ORDER BY CreateTime DESC ";

            return new ResponseResult<IEnumerable<CorporationDto>>(GetAll(SelSql));
        }
    }
}
