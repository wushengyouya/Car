using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface IOrderInfoService:IBaseService<OrderInfo>
    {
        List<OrderInfo> GetOrdersList();
        /// <summary>
        /// 根据关键词搜索订单
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        List<OrderInfo> GetOrdersList(string keyword);
        /// <summary>
        /// 查找用户所有订单
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        List<OrderInfo> GetOrdersListByUserId(int userId);
        /// <summary>
        /// 查找用户不同状态的订单
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="orderFlag">订单状态</param>
        /// <returns></returns>
        List<OrderInfo> GetOrdersListByUserId(int userId,int orderFlag);

        /// <summary>
        /// 通过订单状态查
        /// </summary>
        /// <param name="orderFlag"></param>
        /// <returns></returns>
        List<OrderInfo> GetOrdersListByOrderFlag(int orderFlag);

        /// <summary>
        /// 查找一个订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        OrderInfo GetOrdersByOrderId(string orderId);
    }
}
