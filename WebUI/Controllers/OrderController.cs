using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using BLLFactory;
using Model;
using WebUI.Models;

namespace WebUI.Controllers
{
    [RoutePrefix("order")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //所有订单
        [Route("all")]
        public ActionResult AllOrders()
        {

            var orderInfos = _unitOfWork.OrderInfoService.GetOrdersList();
            //_unitOfWork.UserInfoService.GetUserInfoById()

            var ordersModels = new List<AllOrdersModel>();

            foreach (var item in orderInfos)
            {
                AllOrdersModel allOrdersModel = new AllOrdersModel
                {
                    OrderExpressId = item.order_express_id,
                    OrderFlag = Tool.OrderFlagToString((Tool.OrderFlag)item.order_flag),
                    OrderId = item.order_id,
                    OrderTime = item.order_time,
                    OrderTotalMoney = item.order_totalMoney,
                    UserName = _unitOfWork.UserInfoService.GetUserInfoById(item.user_id).user_name

                };
                ordersModels.Add(allOrdersModel);
            }
            return View(ordersModels);
        }


        //待发货订单
        [Route("staysendout")]
        [HttpGet]
        public ActionResult StaySendOutOrders()
        {
            var orderInfos = _unitOfWork.OrderInfoService.GetOrdersListByOrderFlag((int)Tool.OrderFlag.StaySendOut).OrderByDescending(m=>m.order_time);

            var ordersModels = new List<AllOrdersModel>();

            foreach (var item in orderInfos)
            {
                AllOrdersModel allOrdersModel = new AllOrdersModel
                {
                    OrderExpressId = item.order_express_id,
                    OrderFlag = Tool.OrderFlagToString((Tool.OrderFlag)item.order_flag),
                    OrderId = item.order_id,
                    OrderTime = item.order_time,
                    OrderTotalMoney = item.order_totalMoney,
                    UserName = _unitOfWork.UserInfoService.GetUserInfoById(item.user_id).user_name

                };
                ordersModels.Add(allOrdersModel);
            }
            return View(ordersModels);
        }


        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="expressId">快递编号</param>
        /// <returns></returns>
        [Route("staysendout")]
        [HttpPost]
        public JsonResult StaySendOutOrders(string orderId, string expressId)
        {
            var order = _unitOfWork.OrderInfoService.GetOrdersByOrderId(orderId);

            var ordersModels = new List<AllOrdersModel>();
            if (order != null)
            {
                order.order_flag = (int?)Tool.OrderFlag.StayReceiving;
                order.order_express_id = expressId;

                if (_unitOfWork.OrderInfoService.EditEntity(order))
                {
                    var orderInfos = _unitOfWork.OrderInfoService.GetOrdersListByOrderFlag((int)Tool.OrderFlag.StaySendOut);
                    foreach (var item in orderInfos)
                    {
                        AllOrdersModel allOrdersModel = new AllOrdersModel
                        {
                            OrderExpressId = item.order_express_id,
                            OrderFlag = Tool.OrderFlagToString((Tool.OrderFlag)item.order_flag),
                            OrderId = item.order_id,
                            OrderTime = item.order_time,
                            OrderTotalMoney = item.order_totalMoney,
                            UserName = _unitOfWork.UserInfoService.GetUserInfoById(item.user_id).user_name

                        };
                        ordersModels.Add(allOrdersModel);
                    }

                    return Json(new { Msg = "ok", Data = ordersModels });
                }

            }

            return Json(new { Msg = "no" });
        }

       
        //订单详情
        [Route("detail/{orderId}")]
        public ActionResult UserOrderDetail(string orderId)
        {
            Session["text"] = "订单详情";
            OrderInfo orderInfo = _unitOfWork.OrderInfoService.GetOrdersByOrderId(orderId);
            OrderDetailModel orderDetailModel = new OrderDetailModel
            {
                UserInfo = orderInfo.UserInfo,
                OrderInfo = orderInfo,
                OrderGoodses = orderInfo.Order_Goods.ToList()
            };
            return View(orderDetailModel);
        }

        //搜索订单
        [Route("search")]
        public ActionResult SearchOrders(string keyword)
        {
            Session["text"] = "搜索订单";
            List<OrderInfo> orderInfos = _unitOfWork.OrderInfoService.GetOrdersList(keyword);

            if (!orderInfos.Any())
            {
                return View(new List<OrderInfo>());
            }

            return View(orderInfos);
        }


    }
}