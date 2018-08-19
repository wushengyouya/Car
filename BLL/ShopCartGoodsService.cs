using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using IDAL;
using Model;

namespace BLL
{
    public class ShopCartGoodsService : BaseService<ShopCart_Goods>, IShopCartGoodsService
    {
        public override void SetDal()
        {
            CurrentDal = DbSession.ShopCartGoodsDal;
        }


        public bool AddGoodsToShopCart(ShopCart_Goods shopCartGoods, int quantity)
        {

            if (LoadEntities(m => m.shopCart_id == shopCartGoods.shopCart_id && m.goods_id == shopCartGoods.goods_id).FirstOrDefault() != null)
            {
                shopCartGoods = LoadEntities(m =>
                    m.shopCart_id == shopCartGoods.shopCart_id && m.goods_id == shopCartGoods.goods_id).FirstOrDefault();
                shopCartGoods.goods_count += quantity;
                return EditEntity(shopCartGoods);
            }

            return AddEntity(shopCartGoods) != null;
        }

        /// <summary>
        /// 根据用户id和购物车id获取商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shopCartId"></param>
        /// <returns></returns>
        public ShopCart_Goods GetShopCartGoods(int id, int shopCartId)
        {
            return LoadEntities(m => m.goods_id == id && m.shopCart_id == shopCartId).FirstOrDefault();
        }

        /// <summary>
        /// 删除多个购物车商品
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteShopCartGoods(List<ShopCart_Goods> list)
        {
            bool b = false;
            foreach (ShopCart_Goods item in list)
            {
                b = DeleteEntity(item);
            }

            return b;
        }

        public bool DeleteShopCartGoods(ShopCart_Goods shopCartGoods)
        {
            return false;
        }

        /// <summary>
        /// 获取用户购物车商品
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ShopCart_Goods> GetShopCartGoods(int userId)
        {
            return LoadEntities(m => m.shopCart_id == userId).ToList();
        }

        public List<ShopCart_Goods> GetShopCartGoods()
        {
            return LoadEntities(m => true).ToList();
        }
    }
}
