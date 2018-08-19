using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface ICollectGoodsService:IBaseService<CollectGoods>
    {
       
        /// <summary>
        /// 判断一个用户是否收藏了某个商品
        /// </summary>
        /// <param name="userInfo">当前用户信息</param>
        /// <param name="goodsId">商品编号</param>
        /// <returns></returns>
        bool UserIsCollectGoods(UserInfo userInfo, int goodsId);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        List<CollectGoods> GetCollectGoods(int userId);
    }
}
