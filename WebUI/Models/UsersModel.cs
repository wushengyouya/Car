using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class UsersModel
    {
        public string UserHeadPortrait { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserTel { get; set; }
        public string UserSex { get; set; }
        public string UserFlag { get; set; }
        public DateTime? RegistTime { get; set; }


        public string UserAdress { get; set; }
    }
}