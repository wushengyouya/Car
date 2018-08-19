using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    //数据会话层接口
    public interface IDbSession
    {
        DbContext Db { get; }
        IUserInfoDal UserInfoDal { get; set; }
        IGoodsTypeDal GoodsTypeDal { get; set; }
        IGoodsDal GoodsDal { get; set; }
        IPictureDal PictureDal { get; set; }
        IShopCartGoodsDal ShopCartGoodsDal { get; set; }
        ICollectGoodsDal CollectGoodsDal { get; set; }
        IOrderInfoDal OrderInfoDal { get; set; }
        IOrderGoodsDal OrderGoodsDal { get; set; }
        ICommentDal CommentDal { get; set; }
        IAdminDal AdminDal { get; set; }
        bool SaveChanges();
        
    }
}
