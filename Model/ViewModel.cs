using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ViewModel
    {
        public List<UserInfo> UserInfosList { get; set; }
        public List<Admin> AdminsList { get; set; }
       // public List<Collect> CollectsList { get; set; }
        public List<CollectGoods> CollectGoodsesList { get; set; }
        public List<Comment> CommentsList { get; set; }
        public List<Goods> GoodsList { get; set; }
        public List<GoodsType> GoodsTypeList { get; set; }
        public List<Order_Goods> OrderGoodsList { get; set; }
        public List<OrderInfo> OrderInfoList { get; set; }
        public List<Picture> PictureList { get; set; }
       // public List<ShopCart> ShopCart { get; set; }
        public List<ShopCart_Goods> ShopCartGoodsList { get; set; }
    }
}
