using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using Model;


namespace BLL
{
    public class OrderInfoService:BaseService<OrderInfo>,IOrderInfoService
    {
      

        public override void SetDal()
        {
            CurrentDal = DbSession.OrderInfoDal;
        }

        public List<OrderInfo> GetOrdersList()
        {
            return LoadEntities(m => true).OrderByDescending(m => m.order_time).ToList();
        }

        /// <summary>
        /// 搜索订单先精确搜索，再进行模糊搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<OrderInfo> GetOrdersList(string keyword)
        {
            IEnumerable<OrderInfo> orderInfos = LoadEntities(m => m.order_id.Equals(keyword));
            if (!orderInfos.Any())
            {
                orderInfos = LoadEntities(m => m.Order_Goods.Any(g=>g.Goods.goods_name.Equals(keyword)));
                if (!orderInfos.Any())
                {
                    orderInfos = LoadEntities(m => m.UserInfo.user_name.Equals(keyword));
                    if (!orderInfos.Any())
                    {
                        orderInfos = LoadEntities(m => m.order_express_id.Contains(keyword))
                            .Concat(LoadEntities(m => m.UserInfo.user_name.Contains(keyword)));
                    }
                }
            }

            return orderInfos.ToList();
        }

        public List<OrderInfo> GetOrdersListByUserId(int userId)
        {
            return LoadEntities(m => m.user_id == userId).OrderByDescending(m=>m.order_time).ToList();
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="orderFlag">订单状态</param>
        /// <returns></returns>
        public List<OrderInfo> GetOrdersListByUserId(int userId, int orderFlag)
        {
            return LoadEntities(m => m.user_id == userId && m.order_flag == orderFlag).OrderByDescending(m => m.order_time).ToList();
        }

        /// <summary>
        /// 通过订单状态
        /// </summary>
        /// <param name="orderFlag"></param>
        /// <returns></returns>
        public List<OrderInfo> GetOrdersListByOrderFlag(int orderFlag)
        {
            return LoadEntities(m => m.order_flag==orderFlag).ToList();
        }

        /// <summary>
        /// 通过订单编号获取订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public OrderInfo GetOrdersByOrderId(string orderId)
        {
            return LoadEntities(m => m.order_id.Equals(orderId)).FirstOrDefault();
        }
    }
}
