using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface IShopCartGoodsService:IBaseService<ShopCart_Goods>
    {
        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <param name="shopCartGoods"></param>
        /// <returns></returns>
        bool AddGoodsToShopCart(ShopCart_Goods shopCartGoods, int quantity);

        /// <summary>
        /// 获取用户的购物车商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shopCartId"></param>
        /// <returns></returns>
        ShopCart_Goods GetShopCartGoods(int id, int shopCartId);

        /// <summary>
        /// 删除多个购物车商品
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeleteShopCartGoods(List<ShopCart_Goods> list);

        /// <summary>
        /// 删除单个购物车商品
        /// </summary>
        /// <param name="shopCartGoods"></param>
        /// <returns></returns>
        bool DeleteShopCartGoods(ShopCart_Goods shopCartGoods);

        /// <summary>
        /// 获取用户购物车所有商品
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<ShopCart_Goods> GetShopCartGoods(int userId);
        /// <summary>
        /// 获取所有购物车商品
        /// </summary>
        /// <returns></returns>
        List<ShopCart_Goods> GetShopCartGoods();
    }
}
