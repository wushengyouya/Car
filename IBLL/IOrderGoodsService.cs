using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface IOrderGoodsService:IBaseService<Order_Goods>
    {
        /// <summary>
        /// 查找所有已售商品
        /// </summary>
        /// <returns></returns>
        List<Order_Goods> GetOrderGoodses();
    }
}
