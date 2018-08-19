using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebUI.Models
{

    public class GoodsModel
    {
        public bool UserIsCollectGoods { get; set; }
        public Goods Goods { get; set; }
    }

    public class IndexModel
    {
        public GoodsType GoodsType { get; set; }
        public List<GoodsModel> Goodses { get; set; }
        

    }
}