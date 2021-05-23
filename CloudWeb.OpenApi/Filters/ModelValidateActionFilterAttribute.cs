using CloudWeb.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace CloudWeb.OpenApi.Filters
{
    /// <summary>
    /// 模型验证
    /// </summary>
    public class ModelValidateActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                //公共返回数据类
                ResponseResult returnMsg = new ResponseResult() { code = (int)HttpStatusCode.fail };

                //获取具体的错误消息
                foreach (var item in context.ModelState.Values)
                {
                    
                    //遍历所有项目的中的所有错误信息
                    foreach (var err in item.Errors)
                    {
                        //消息拼接,用|隔开，前端根据容易解析
                        returnMsg.msg += $"{err.ErrorMessage}|";
                    }
                }
                context.Result = new JsonResult(returnMsg);
            }

        }
    }
}
