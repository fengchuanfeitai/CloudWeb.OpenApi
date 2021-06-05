using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using System.Collections.Generic;
using CloudWeb.Util;
using CloudWeb.Dto.Param;
using System;

namespace CloudWeb.Services
{
    public class CorporationService : BaseDao, ICorporationService
    {

        #region 私有方法

        private string GetColTxt(string colStr)
        {
            var ColTxt = "";
            if (colStr == null)
                return ColTxt;

            var sql = $"SELECT * FROM dbo.[Columns] WHERE IsDel=0 AND IsShow=1 AND ColumnId IN ({colStr})";
            var Columns = GetAll<ColumnDto>(sql, new { colStr = colStr });

            foreach (var item in Columns)
            {
                ColTxt += item.ColName + " ";
            }
            return ColTxt;
        }

        private int GetSort()
        {
            var MaxSortsql = "SELECT ISNULL(MAX(Sort),0) FROM dbo.Corporations";
            return MaxSort(MaxSortsql) + 1;
        }

        private CorporationDto GetCorpByName(string name)
        {
            var sql = "SELECT * FROM dbo.Corporations WHERE  IsDel = 0 AND IsShow = 1 AND [Name] = @Name";
            var Column = Find<CorporationDto>(sql, new { Name = name });
            return Column;
        }

        #endregion

        #region 公用方法
        #endregion

        #region 后台接口

        /// <summary>
        /// 添加公司信息
        /// </summary>
        /// <param name="corporation"></param>
        /// <returns></returns>
        public ResponseResult<bool> AddCorporation(CorporationDto corporation)
        {
            var result = new ResponseResult<bool>();
            if (corporation == null)
                return result.SetFailMessage("添加公司错误。公司信息不存在");
            var corp = GetCorpByName(corporation.Name);
            if (corp != null)
                return result.SetFailMessage("公司名不能重复，请更换公司名！");

            corporation.CreateTime = DateTime.Now;
            corporation.ModifyTime = corporation.CreateTime;
            corporation.Modifier = corporation.Creator;
            corporation.IsDel = 0;
            if (corporation.Sort == null)
            {
                corporation.Sort = GetSort();
            }

            const string InsertSql = @"INSERT INTO dbo.Corporations
               (CreateTime,ModifyTime,Creator,Modifier,[Name],Cover,Logo1,Logo2,ColumnId,
                AboutUs,AboutUsCover,ContactUs,ContactUsBg,Sort,IsShow,IsDel)
                VALUES
                (@CreateTime,@ModifyTime,@Creator,@Modifier,@Name,@Cover,@Logo1,@Logo2,@ColumnId,
                 @AboutUs,@AboutUsCover,@ContactUs,@ContactUsBg,@Sort,@IsShow,@IsDel)";

            return new ResponseResult<bool>(Add(InsertSql, corporation));
        }

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ResponseResult<bool> DelCorporation(int[] ids)
        {
            var result = new ResponseResult<bool>();
            if (ids.Length == 0)
                return result.SetFailMessage("请选择公司！");

            string sql = "";
            string idsStr = ConverterUtil.StringSplit(ids);
            if (ids.Length > 0)
                sql = $"UPDATE dbo.CorpProducts SET IsDel = 1 WHERE IsDel=0 AND CorpId = {idsStr} ";

            string delSql = $"UPDATE dbo.Corporations SET IsDel = 1 WHERE CorpId in ({idsStr}) {sql}";

            return result.SetData(Delete(delSql, new { ids = ids }));
        }

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="corporation"></param>
        /// <returns></returns>
        public ResponseResult<bool> UpdateCorporation(CorporationDto corporation)
        {
            var result = new ResponseResult<bool>();
            var Corporation = GetCorporation(corporation.CorpId.Value);
            if (Corporation.data == null)
                return result.SetFailMessage("修改失败，公司信息不存在。");
            if (Corporation.data.Name != corporation.Name)
            {
                var corp = GetCorpByName(corporation.Name);
                if (corp != null)
                    return result.SetFailMessage("公司名不能重复，请更换公司名！");
            }

            if (Equals(corporation, Corporation))
                return result.SetData(true);

            corporation.ModifyTime = DateTime.Now;
            corporation.Modifier = corporation.Creator;
            const string UpdateSql = @"UPDATE dbo.Corporations SET ModifyTime=@ModifyTime,Modifier=@Modifier,
                    [Name]=@Name,Cover=@Cover,Logo1=@Logo1, Logo2=@Logo2,ColumnId=@ColumnId,AboutUs=@AboutUs,
                    AboutUsCover=@AboutUsCover,ContactUs=@ContactUs,ContactUsBg=@ContactUsBg,Sort=@Sort,
                    IsShow=@IsShow,IsDel=@IsDel
                    WHERE IsDel=0 AND CorpId=@CorpId";

            return new ResponseResult<bool>(Update(UpdateSql, corporation));
        }

        public ResponseResult<CorporationDto> GetCorporation(int id)
        {
            const string SelSql = @"SELECT CorpId,CreateTime,ModifyTime,Creator,Modifier,
                [Name],Cover,Logo1,Logo2,ColumnId,AboutUs,AboutUsCover,ContactUs,ContactUsBg,
                Sort,IsShow,IsDel FROM dbo.Corporations
                WHERE IsDel=0 AND CorpId=@id ORDER BY CreateTime DESC";

            return new ResponseResult<CorporationDto>(Find<CorporationDto>(SelSql, new { id = id }));
        }

        /// <summary>
        /// 分页查询公司信息
        /// </summary>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public ResponseResult<IEnumerable<CorporationDto>> GetPageList(CorpSearchParam pageParam)
        {
            string SearchName = "";
            if (!string.IsNullOrEmpty(pageParam.CorpNameKeyword))
                SearchName = " AND Name like '%" + pageParam.CorpNameKeyword + "%'";

            string SelSql = $"SELECT c2.[Index],c1.CorpId,c1.[Name],c1.ColumnId,c1.Sort,c1.IsShow,c1.CreateTime FROM dbo.Corporations c1,(SELECT TOP (@PageIndex*@PageSize) ROW_NUMBER() OVER(ORDER BY CreateTime DESC ) [Index],CorpId FROM dbo.Corporations) c2 WHERE c1.CorpId = c2.CorpId {SearchName} AND c1.IsDel = 0 AND c2.[Index] >(@PageSize*(@PageIndex-1)) ORDER BY c2.[Index] ASC";

            const string CountSql = @"SELECT COUNT(*) FROM dbo.Corporations WHERE IsDel=0";
            var List = GetAll<CorporationDto>(SelSql, pageParam);

            foreach (var corp in List)
            {
                corp.ColTxtName = GetColTxt(corp.ColumnId);
            }
            //处理栏目Id多个
            return new ResponseResult<IEnumerable<CorporationDto>>(List, Count(CountSql));
        }

        /// <summary>
        /// 改变显示状态
        /// </summary>
        /// <param name="showStatusParam">状态参数</param>
        /// <returns></returns>
        public ResponseResult ChangeShowStatus(ShowStatusParam showStatusParam)
        {
            ResponseResult result = new ResponseResult();
            string sql = "UPDATE dbo.Corporations SET IsShow = @ShowStatus WHERE CorpId = @Id";
            bool isSuccess = Update(sql, showStatusParam);

            if (isSuccess)
                result.Set((int)HttpStatusCode.OK, "修改状态成功");
            else
                result.Set((int)HttpStatusCode.fail, "修改状态失败");
            return result;
        }
        public ResponseResult<IEnumerable<CorporationDto>> GetCorpSelectList()
        {
            const string sql = @"SELECT CorpId,[Name] FROM dbo.Corporations WHERE IsShow=1 AND IsDel=0";

            return new ResponseResult<IEnumerable<CorporationDto>>(GetAll<CorporationDto>(sql));

        }

        #endregion

        #region 网站接口

        /// <summary>
        /// 展示所有公司
        /// </summary>
        /// <returns></returns>
        public ResponseResult<IEnumerable<CorporationDto>> GetAllCorp()
        {
            string sql = "SELECT * FROM dbo.Corporations WHERE IsShow = 1 AND IsDel = 0 ORDER BY Sort ASC;";

            var corporation = GetAll<CorporationDto>(sql);

            foreach (var item in corporation)
            {
                item.DisPlayIndex = 1;
            }
            return new ResponseResult<IEnumerable<CorporationDto>>(corporation);
        }

        public ResponseResult<IEnumerable<CorporationDto>> GetCorpByCol(CorpByColParam param)
        {
            var result = new ResponseResult<IEnumerable<CorporationDto>>();
            string sql = "Select * From Corporations Where IsDel = 0 AND IsShow = 1 AND CHARINDEX(',6,', ',' + cast(ColumnId as varchar) + ',') > 0";

            var corps = GetAll<CorporationDto>(sql, param);
            if (corps == null || param.TitleCut == null)
                return result.SetData(corps);

            foreach (var item in corps)
            {
                item.TruncatName = TextUtil.StringTruncat(item.Name, param.TitleCut.Value, "");
            }

            return result.SetData(corps);
        }

        #endregion

    }
}
