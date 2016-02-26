using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PM24.Models
{
    public class UserInfo
    {
        //用户ID
        //以字母开头，长度在6~18之间,只能包含字符、数字、下划线
       [RegularExpression(@"^[a-zA-z][a-zA-Z0-9_]{5,17}$", ErrorMessage = "*")]
       [Required(ErrorMessage = "*")]
        public  string UserId { get; set; }
        //密码
        //长度在6~18之间,只能包含字符、数字、特殊字符
        [RegularExpression(@"^[A-Za-z0-9!#$%^&*.~/?,!\\@_]{6,18}$", ErrorMessage = "*")]
        [Required (ErrorMessage="*")]
        public string Pwd { get; set; }
        //昵称
        [StringLength(10,ErrorMessage="*")]
        public string Name { get; set; }
        //邮箱
        [RegularExpression(@"[a-zA-Z0-9\-\._]+@[a-zA-Z0-9\-_]+(\.[a-zA-Z]+)+",ErrorMessage="*")]
        public string Email { get; set; }
        //创建日期
        public DateTime CreateDate { get; set; }
        //创建电脑IP
        public string CreateIp { get; set; }
        //用户状态
        public int Status { get; set; }
    }
}