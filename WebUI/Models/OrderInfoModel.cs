using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebUI.Models
{
    public class OrderInfoModel
    {
        public string OrderId { get; set; }
        public int? UserId { get; set; }
        public decimal? OrderTotalMoney { get; set; }
        public DateTime? OrderTime { get; set; }
        public string OrderExpressId { get; set; }
        public string OrderFlag { get; set; }

       
        public virtual ICollection<Order_Goods> OrderGoods { get; set; }
    }
}