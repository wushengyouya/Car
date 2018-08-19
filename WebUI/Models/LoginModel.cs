using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public bool RemPwd { get; set; }
        public string ValidateCode { get; set; }      

    }
}