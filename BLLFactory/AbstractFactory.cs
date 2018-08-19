using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BLL;
using IBLL;

namespace BLLFactory
{
    public class AbstractFactory
    {
        private static readonly string  _Assembly = ConfigurationManager.AppSettings["BLL_Assembly"];
        private static readonly string _NameSpace = ConfigurationManager.AppSettings["BLL_NameSpace"];

        public static IUserInfoService GetUserInfoService()
        {
            string typeName = _NameSpace+".UserInfoService";
            return CreateInstance(typeName) as IUserInfoService;
        }

        public static IGoodsTypeService GetGoodsTypeService()
        {
            string typeName = _NameSpace + ".GoodsTypeService";
            return CreateInstance(typeName) as IGoodsTypeService;
        }

        public static IGoodsService GetGoodsService()
        {
            string typeName = _NameSpace + ".GoodsService";
            return CreateInstance(typeName) as IGoodsService;
        }

        
        /// <summary>
        /// 图片
        /// </summary>
        /// <returns></returns>
        public static IPictureService GetPictureService()
        {
            string typeName = _NameSpace + ".PictureService";
            return CreateInstance(typeName) as IPictureService;
        }

        /// <summary>
        /// 购物车
        /// </summary>
        /// <returns></returns>
        public static IShopCartGoodsService GetShopCartGoodsService()
        {
            string typeName = _NameSpace + ".ShopCartGoodsService";
            return CreateInstance(typeName) as IShopCartGoodsService;
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <returns></returns>
        public static ICollectGoodsService GetCollectGoodsService()
        {
            string typeName = _NameSpace + ".CollectGoodsService";
            return CreateInstance(typeName) as ICollectGoodsService;
        }

        
        /// <summary>
        /// 订单
        /// </summary>
        /// <returns></returns>
        public static IOrderInfoService GetOrderInfoService()
        {
            string typeName = _NameSpace + ".OrderInfoService";
            return CreateInstance(typeName) as IOrderInfoService;
        }

        /// <summary>
        /// 订单商品
        /// </summary>
        /// <returns></returns>
        public static IOrderGoodsService GetOrderGoodsService()
        {
            string typeName = _NameSpace + ".OrderGoodsService";
            return CreateInstance(typeName) as IOrderGoodsService;
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        public static ICommentService GetCommentService()
        {
            string typeName = _NameSpace + ".CommentService";
            return CreateInstance(typeName) as ICommentService;
        }
        /// <summary>
        /// 创建业务层实例
        /// </summary>
        /// <param name="bllName"></param>
        /// <returns></returns>
        private static object CreateInstance(string bllName)
        {
            Assembly assembly = Assembly.Load(_Assembly);
            return Activator.CreateInstance(assembly.GetType(bllName));
        }

    }
}
