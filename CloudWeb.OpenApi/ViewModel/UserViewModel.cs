using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CloudWeb.OpenApi.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(16, ErrorMessage = "不能超过{0}个字符")]
        public string UserName { get; set; }
    }
}
