using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ShinePhoto.Models
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "密码必须填写")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "密码必须填写")]
        public string Password { get; set; }
    }
}
