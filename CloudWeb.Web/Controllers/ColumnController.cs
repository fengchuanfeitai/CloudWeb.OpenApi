using CloudWeb.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CloudWeb.Web.Controllers
{
    public class ColumnController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public JsonResult UploadImg()
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                var files = Request.Form.Files;

                if (files != null && files.Count > 0)
                {
                    var file = files[0];
                    string getex = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
                    if (FileUpLoad.IsImgFile(getex))
                    {
                        string path = Server.MapPath("~/Upload/");
                        string newName = FileUpLoad.CreateFileName(getex);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }


                        using (var stream = new FileStream(path, FileMode.Create))
                        {

                            //图片路径保存到数据库里面去
                            //bool flage = QcnApplyDetm.FinancialCreditQcnApplyDetmAdd(EntId, CrtUser, new_path);
                            //if (flage == true)
                            //{
                            //再把文件保存的文件夹中
                            file.CopyTo(stream);
                            path = path + newName;

                        }

                        responseResult.ResultFlag = 0;
                        responseResult.ResultData = "/Upload/" + newName;
                    }
                    else
                    {
                        responseResult.ResultFlag = -1;
                        responseResult.ResultMessage = "不是有效的文件";
                    }

                }
            }
            catch (Exception e)
            {
                responseResult.ResultFlag = -1;
                responseResult.ResultMessage = "上传失败！";
            }
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

    }
}
