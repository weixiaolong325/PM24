using PM24.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PM24.ViewModes
{
    public class UserInfoView
    {
        //用户信息
        public UserInfo userInfo { get; set; }
        //再次确认密码
        [Required(ErrorMessage="*")]
        public string Pwd2 { get; set; }
        //验证码
        [Required(ErrorMessage="*")]
        public string validateCode { get; set; }
    }
}