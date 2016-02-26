using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PM24.ViewModes
{
    public class UserLoginView
    {
        [Required(ErrorMessage="*")]
        public string UserId { get; set; }
        [Required(ErrorMessage="*")]
        public string Pwd { get; set; }
    }
}