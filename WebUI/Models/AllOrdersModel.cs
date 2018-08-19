using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class AllOrdersModel
    {
        public string OrderId { get; set; }
        public string UserName { get; set; }
        public decimal? OrderTotalMoney { get; set; }
        public System.DateTime? OrderTime { get; set; }
        public string OrderExpressId { get; set; }
        public string OrderFlag { get; set; }
    }
}