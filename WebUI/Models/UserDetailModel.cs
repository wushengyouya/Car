using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebUI.Models
{
    public class UserDetailModel
    {
        public UserInfo UserInfo { get; set; }
        public List<Comment> Comments { get; set; }
        public List<ShopCart_Goods> ShopCartGoodses { get; set; }
        public List<OrderInfo> OrderInfos { get; set; }
    }
}