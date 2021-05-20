﻿using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CloudWeb.OpenApi.Filters
{
    /// <summary>
    /// 给api返回结果包一层状态码
    /// </summary>
    public class ApiResultFilter : IResultFilter
    {
        /// <summary>
        /// 执行结果过滤器
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result != null)
            {
                if (context.Result is ObjectResult objectResult)
                {
                    if (objectResult.DeclaredType is null) //返回的是IActionResult类型
                    {
                        context.Result = new JsonResult(new
                        {
                            status = objectResult.StatusCode,
                            data = objectResult.Value
                        });
                    }
                    else//返回的是string、List这种其他类型，此时没有statusCode，应尽量使用IActionResult类型
                    {




                        context.Result = new JsonResult(objectResult.Value);
                    }
                }
                else if (context.Result is EmptyResult)
                {
                    Meta meta = new Meta(404, "资源不存在");
                    context.Result = new JsonResult("");
                }
                else
                {
                    throw new Exception($"未经处理的Result类型：{ context.Result.GetType().Name}");
                }

            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
