using System;
using System.Configuration;
using System.Reflection;
using DAL;
using IDAL;

namespace DALFactory
{
    public class AbstractFactory
    {
        //数据层的程序集名称
        private static readonly string _Assembly = ConfigurationManager.AppSettings["DAL_Assembly"];

        //数据层的命名空间
        private static readonly string _NameSpace = ConfigurationManager.AppSettings["DAL_NameSpace"];


        public static IUserInfoDal GetUserInfoDal()
        {
            string typeName = _NameSpace+".UserInfoDal";
            return CreateInstance(typeName) as IUserInfoDal;
        }

        /// <summary>
        /// 商品类别数据操作对象
        /// </summary>
        /// <returns></returns>
        public static IGoodsTypeDal GetGoodsTypeDal()
        { 
            string typeName = _NameSpace + ".GoodsTypeDal";
            return CreateInstance(typeName) as IGoodsTypeDal;
        }

      
        /// <summary>
        /// 商品数据操作对象
        /// </summary>
        /// <returns></returns>
        public static IGoodsDal GetGoodsDal()
        {
            string typeName = _NameSpace + ".GoodsDal";
            return CreateInstance(typeName) as  IGoodsDal;
        }
        

        /// <summary>
        /// 商品图片
        /// </summary>
        /// <returns></returns>
        public static IPictureDal GetPictureDal()
        {
            string typeName = _NameSpace + ".PictureDal";
            return CreateInstance(typeName) as IPictureDal;
        }
        
        /// <summary>
        /// 购物车
        /// </summary>
        /// <returns></returns>
        public static IShopCartGoodsDal GetShopCartGoodsDal()
        {
            string typeName = _NameSpace + ".ShopCartGoodsDal";
            return CreateInstance(typeName) as IShopCartGoodsDal;
        }

        

        /// <summary>
        /// 收藏
        /// </summary>
        /// <returns></returns>
        public static ICollectGoodsDal GetCollectGoodsDal()
        {
            string typeName = _NameSpace + ".CollectGoodsDal";
            return CreateInstance(typeName) as ICollectGoodsDal;
        }

        
        /// <summary>
        /// 订单
        /// </summary>
        /// <returns></returns>
        public static IOrderInfoDal GetOrderInfoDal()
        {
            string typeName = _NameSpace + ".OrderInfoDal";
            return CreateInstance(typeName) as IOrderInfoDal;
        }

        /// <summary>
        /// 订单商品
        /// </summary>
        /// <returns></returns>
        public static IOrderGoodsDal GetOrderGoodsDal()
        {
            string typeName = _NameSpace + ".OrderGoodsDal";
            return CreateInstance(typeName) as IOrderGoodsDal;
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        public static ICommentDal GetCommentDal()
        {
            string typeName = _NameSpace + ".CommentDal";
            return CreateInstance(typeName) as ICommentDal;
        }
        
        /// <summary>
        /// 管理
        /// </summary>
        /// <returns></returns>
        public static IAdminDal GetAdminDal()
        {
            string typeName = _NameSpace + ".AdminDal";
            return CreateInstance(typeName) as AdminDal;
        }

        /// <summary>
        /// 创建数据操作的实例
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static object CreateInstance(string fullName)
        {
            var assembly  = Assembly.Load(_Assembly);
            
            return Activator.CreateInstance(assembly.GetType(fullName));
        }
    }
}
