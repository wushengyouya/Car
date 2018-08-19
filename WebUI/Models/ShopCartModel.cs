using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebUI.Models
{
    public class ShopCartGoodsModel
    {
        public bool UserIsCollectGoods { get; set; }
        public int GoodsId { get; set; }
        public int? GoodsTypeId { get; set; }
        public string GoodsName { get; set; }
        public int GoodsPrice { get; set; }
        public int GoodsFlag { get; set; }
        public string GoodsInfo { get; set; }
        public int GoodsCount { get; set; }
        public System.DateTime GoodsAddTime { get; set; }
        public string GoodsTitle { get; set; }
        public string GoodsPictureAdress { get; set; }

        public int ShopCartGoodsCount { get; set; }
        public int TotalMoney { get; set; }
    }
    public class ShopCartModel
    {
        public List<ShopCartGoodsModel> GoodsModels { get; set; }
        
        public int SumGoodsCount { get; set; }
        public int SumPrice { get; set; }
    }
}