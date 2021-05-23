using CloudWeb.Dto.Common;
using CloudWeb.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;

        public UploadController(ILogger<UploadController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Route("api/admin/upload")]
        public ResponseResult<string> UploadImg()
        {
            ResponseResult<string> responseResult = new ResponseResult<string>();
            try
            {
                var files = Request.Form.Files;

                if (files != null && files.Count > 0)
                {
                    var file = files[0];
                    string getex = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();

                    if (FileUpLoad.IsImgFile(getex))
                    {
                        string path = string.Concat(AppContext.BaseDirectory, "~/Upload/");
                        string newName = FileUpLoad.CreateFileName(getex);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            path = path + newName;
                        }

                        responseResult.code = 0;
                        responseResult.data = "/Upload/" + newName;
                    }
                    else
                    {
                        responseResult.code = -1;
                        responseResult.msg = "不是有效的文件";
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"文件上传失败：{e.Message}");
                responseResult.code = -1;
                responseResult.msg = "上传失败！";
            }
            return responseResult;
        }

    }
}
