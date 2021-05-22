using CloudWeb.Dto.Common;
using CloudWeb.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    public class UploadController : Controller
    {
        [Produces("application/json")]
        [Route("api/admin/[controller]/[action]")]
        [HttpPost]
        public ResponseResult<string> UploadImg(string file)
        {
            ResponseResult<string> responseResult = new ResponseResult<string>();
            try
            {
                FileInfo fi = new FileInfo(file);
                string ext = fi.Extension;
                var orgFileName = fi.Name;
                var newFileName = Guid.NewGuid() + ext;
                var filePath = Path.Combine("", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                     //formFile.CopyToAsync(stream);
                }


                //if (files != null && files.Count > 0)
                //{
                //    var file = files[0];
                //    string getex = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
                //    if (FileUpLoad.IsImgFile(getex))
                //    {
                //        string path = Server.MapPath("~/Upload/");
                //        string newName = FileUpLoad.CreateFileName(getex);
                //        if (!Directory.Exists(path))
                //        {
                //            Directory.CreateDirectory(path);
                //        }
                //        path = path + newName;
                //        file.SaveAs(path);

                //        responseResult.code = 0;
                //        responseResult.data = "/Upload/" + newName;
                //    }
                //    else
                //    {
                //        responseResult.code = -1;
                //        responseResult.msg = "不是有效的文件";
                //    }

                //}
            }
            catch (Exception e)
            {
                responseResult.code = -1;
                responseResult.msg = "上传失败！";
            }
            return responseResult;
        }
    }
}
