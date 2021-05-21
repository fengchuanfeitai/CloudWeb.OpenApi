using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
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
            const string InsertSql = @"INSERT INTO dbo.Corporations(CreateTime,ModifyTime,Creator,Modifier,
                 [Name],Cover,Logo1,Logo2,ColumnId,AboutUs,AboutUsCover,ContactUs,ContactUsBg,Sort,IsDel)
                 VALUES
                 (@CreateTime,@ModifyTime,@Creator,@Modifier,@Name,@Cover,@Logo1,
                 @Logo2,@ColumnId,@AboutUs,@AboutUsCover,@ContactUs,@ContactUsBg,@Sort,@IsDel)";

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

        public ResponseResult<bool> UpdateCorporation(CorporationDto corporation)
        {
            const string UpdateSql = @"UPDATE dbo.Corporations SET ModifyTime=@ModifyTime,Modifier=@Modifier,
                    [Name]=@Name,Cover=@Cover,Logo1=@Logo1, Logo2=@Logo2,ColumnId=@ColumnId,AboutUs=@AboutUs,
                    AboutUsCover=@AboutUsCover,ContactUs=@ContactUs,ContactUsBg=@ContactUsBg,Sort=@Sort,IsDel=@IsDel
                    WHERE IsDel=0 AND CorpId=@CorpId";

            return new ResponseResult<bool>(Update(UpdateSql, corporation));
        }

        public ResponseResult<CorporationDto> GetCorporation(int id)
        {
            const string SelSql = @"SELECT CorpId,CreateTime,ModifyTime,Creator,Modifier,
                [Name],Cover,Logo1,Logo2,ColumnId,AboutUs,AboutUsCover,ContactUs,ContactUsBg,
                Sort,IsDel FROM dbo.Corporations
                WHERE IsDel=0 AND CorpId=@id ORDER BY CreateTime DESC";

            return new ResponseResult<CorporationDto>(Find(SelSql, id));
        }

        public ResponseResult<IEnumerable<CorporationDto>> GetCorporation()
        {
            const string SelSql = @"SELECT CorpId,CreateTime,ModifyTime,Creator,Modifier,
                [Name],Cover,Logo1,Logo2,ColumnId,AboutUs,AboutUsCover,ContactUs,ContactUsBg,
                Sort,IsDel FROM dbo.Corporations 
                WHERE IsDel=0 ORDER BY CreateTime DESC ";

            return new ResponseResult<IEnumerable<CorporationDto>>(GetAll(SelSql));
        }
    }
}
