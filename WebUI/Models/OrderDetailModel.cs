using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebUI.Models
{
    public class OrderDetailModel
    {
        public UserInfo UserInfo { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public List<Order_Goods> OrderGoodses { get; set; }
    }
}