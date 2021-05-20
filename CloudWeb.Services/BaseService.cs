using CloudWeb.Common.Dto;
using CloudWeb.Dto.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudWeb.Services
{
    public abstract class BaseService<T> where T : class, new()
    {
        public abstract Result<T> GetAll();



    }
}
