using CloudWeb.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace CloudWeb.OpenApi.Filters
{
    /// <summary>
    /// 模型验证
    /// </summary>
    public class ModelValidateActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!context.ModelState.IsValid)
            {
                string error = string.Empty;
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    //获取具体的错误消息
                    if (state.Errors.Any())
                    {
                        //返回第一条错误信息
                        error = state.Errors.First().ErrorMessage;
                        break;
                    }
                }

                //公共返回数据类
                ResponseResult returnMsg = new ResponseResult((int)HttpStatusCode.fail, error);
                context.Result = new JsonResult(returnMsg);
            }

        }
    }
}
