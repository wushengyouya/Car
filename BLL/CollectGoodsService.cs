using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using Model;

namespace BLL
{
    public class CollectGoodsService : BaseService<CollectGoods>, ICollectGoodsService
    {
        public override void SetDal()
        {
            CurrentDal = DbSession.CollectGoodsDal;
        }


        /// <summary>
        /// 判断用户是否收藏过此商品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userInfo"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool UserIsCollectGoods(UserInfo userInfo, int goodsId)
        {

            return userInfo != null && LoadEntities(m => true).Any(m => m.collect_id == userInfo.user_id && m.goods_id == goodsId);
        }

        public List<CollectGoods> GetCollectGoods(int userId)
        {
            return LoadEntities(m => true).Where(m => m.collect_id == userId).ToList();
        }
    }
}
