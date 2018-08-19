using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using Model;

namespace BLL
{
    public class OrderGoodsService:BaseService<Order_Goods>,IOrderGoodsService
    {
        public override void SetDal()
        {
            CurrentDal = DbSession.OrderGoodsDal;
        }

        public List<Order_Goods> GetOrderGoodses()
        {
            return LoadEntities(m => true).ToList();
        }
    }
}
