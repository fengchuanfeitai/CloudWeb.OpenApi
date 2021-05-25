using CloudWeb.Dto.Common;
using CloudWeb.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Text;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadController(ILogger<UploadController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 单文件上传文件
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
                        var picPath = "Upload";//文件上传目录
                        //目录wwwroot
                        string contentRootPath = _hostingEnvironment.WebRootPath;
                        string path = Path.Combine(contentRootPath, picPath);
                        string newName = FileUpLoad.CreateFileName(getex);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //创建文件到目录
                        using (var stream = new FileStream(Path.Combine(path, newName), FileMode.Create))
                        {
                            file.CopyTo(stream);
                            path = path + newName;
                        }

                        responseResult.code = (int)HttpStatusCode.OK;
                        responseResult.data = $"{ GetCompleteUrl()}/{picPath}/{ newName}";//新的图片路径，网络地址
                    }
                    else
                    {
                        responseResult.code = (int)HttpStatusCode.fail;
                        responseResult.msg = "不是有效的文件";
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"文件上传失败：{e.Message}");
                responseResult.code = (int)HttpStatusCode.fail;
                responseResult.msg = "上传失败！";
            }

            return responseResult;
        }

        /// <summary>
        /// 获取当前请求完整的Url地址
        /// </summary>
        /// <returns></returns>
        private string GetCompleteUrl()
        {
            return new StringBuilder()
                 .Append(HttpContext.Request.Scheme)
                 .Append("://")
                 .Append(HttpContext.Request.Host)
                 //.Append(HttpContext.Request.PathBase)
                 //.Append(HttpContext.Request.Path)
                 //.Append(HttpContext.Request.QueryString)
                 .ToString();
        }

    }
}
