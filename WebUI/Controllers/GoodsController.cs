using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using BLLFactory;
using IBLL;
using Model;
using System.Dynamic;
using System.Web.WebPages;
using Common;
using log4net;
using WebUI.Models;

namespace WebUI.Controllers
{


    [RoutePrefix("goods")]
    public class GoodsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public GoodsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Setting.GoodsTypes = _unitOfWork.GoodsTypeService.GetGoodsTypeList();
        }


        [Route("type/{id}")]
        public ActionResult GoodsType(int id = 1)
        {
            List<GoodsModel> goodsModels = new List<GoodsModel>();

            var goods = _unitOfWork.GoodsService.GetGoodsListByTypeId(id);
            GoodsTypeModel goodsTypeModel = new GoodsTypeModel
            {
                GoodsTypeName = _unitOfWork.GoodsTypeService.GetGoodsType(id).goods_type_name
            };
            foreach (var item in goods)
            {
                GoodsModel goodsModel = new GoodsModel
                {
                    Goods = item,
                    UserIsCollectGoods = _unitOfWork.CollectGoodsService.UserIsCollectGoods(CurrentUser, item.goods_id)
                };
                goodsModels.Add(goodsModel);
            }

            goodsTypeModel.Goodses = goodsModels;

            return View(goodsTypeModel);
        }


        /// <summary>
        /// 展示商品详细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("detail/{id}")]
        public ActionResult GoodsDetail(int id = 1)
        {
            Goods goods = null;
            GoodsType goodsType = null;
            try
            {

                goods = _unitOfWork.GoodsService.GetGoods(id);
                goodsType = _unitOfWork.GoodsTypeService.GetGoodsType(goods.goods_type_id);
            }
            catch (Exception e)
            {
                LogHelper.Error("logerror", "未能查到商品,编号:" + id, e);
            }


            //List<CommentModel> commentModels = new List<CommentModel>();

            //最新评论

            GoodsDetailModel goodsDetailModel = new GoodsDetailModel
            {
                UserIsCollectGoods = _unitOfWork.CollectGoodsService.UserIsCollectGoods(CurrentUser, goods.goods_id),
                Goods = goods,
                GoodsType = goodsType,
                CommentsOrderByTime = _unitOfWork.CommentService.GetCommentsOrderByTime(goods.goods_id),
                PictureAdress = goods.Picture.ToList()
            };
            return View(goodsDetailModel);
        }

        /// <summary>
        /// 添加商品至购物车，  收藏商品
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <param name="quantity">商品数量</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GoodsDetail(string action, int id, int? quantity = 1)
        {
            string msg = null;
            switch (action)
            {
                //加入购物车
                case "shopcart":
                    {
                        if (Session["UserInfo"] == null)
                        {
                            return Json("请登录");
                        }

                        if (_unitOfWork.GoodsService.GetGoods(id).goods_count<quantity)
                        {
                            return Json("商品库存不足");
                        }
                        UserInfo userInfo = (UserInfo)Session["UserInfo"];
                        ShopCart_Goods shopCartGoods = new ShopCart_Goods
                        {
                            goods_id = id,
                            goods_AddTime = DateTime.Now.ToLocalTime(),
                            shopCart_id = userInfo.user_id,
                            goods_count = quantity.Value

                        };
                        try
                        {
                            bool result =
                                _unitOfWork.ShopCartGoodsService.AddGoodsToShopCart(shopCartGoods, quantity.Value);
                            if (result)
                            {
                               
                                LogHelper.Info("loginfo",
                                    $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：加入购物车，详情：商品编号{shopCartGoods.goods_id}");
                                return Json("添加成功");
                            }
                            return Json("添加失败");
                        }
                        catch (Exception e)
                        {
                            LogHelper.Error("logerror", "加入购物车异常", e);
                        }
                        break;

                    }
                //收藏商品
                case "collect":
                    {
                        if (Session["UserInfo"] == null)
                        {
                            return Json("请登录");
                        }
                        UserInfo userInfo = (UserInfo)Session["UserInfo"];
                        CollectGoods collectGoods = new CollectGoods
                        {
                            collect_id = userInfo.user_id,
                            collect_time = DateTime.Now.ToLocalTime(),
                            goods_id = id

                        };
                        if (_unitOfWork.CollectGoodsService.UserIsCollectGoods(userInfo, id))
                        {
                            try
                            {
                                _unitOfWork.CollectGoodsService.DeleteEntity(collectGoods);
                                LogHelper.Info("loginfo",
                                    $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：取消收藏，详情：商品编号-{collectGoods.goods_id}");
                            }
                            catch (Exception e)
                            {
                                LogHelper.Error("logerror","取消收藏失败",e);
                            }
                           
                            msg = "no";
                        }
                        else
                        {
                            try
                            {
                                _unitOfWork.CollectGoodsService.AddEntity(collectGoods);
                                LogHelper.Info("loginfo",
                                    $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：收藏，详情：商品编号-{collectGoods.goods_id}");
                            }
                            catch (Exception e)
                            {
                                LogHelper.Error("logerror", "收藏失败", e);
                            }
                            msg = "ok";
                        }
                        return Json(msg);
                    }

            }
            return null;
        }






        [Route("pay/{orderId?}")]
        public ActionResult Pay(string orderId)
        {

            if (Request.IsAjaxRequest())
            {
                //if (CurrentUser.user_adress.IsEmpty())
                //{
                //    return Json("no");如果收货地址为空，拒绝支付
                //}
                OrderInfo order = _unitOfWork.OrderInfoService.GetOrdersByOrderId(orderId);
                order.order_flag = (int)Tool.OrderFlag.StaySendOut;

                try
                {
                    var result = _unitOfWork.OrderInfoService.EditEntity(order);
                    if (result)
                    {
                        LogHelper.Info("loginfo",
                            $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：付款，详情：订单编号-{order.order_id}");
                        return Json("支付成功");
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Error("logerror", "支付失败", e);
                }
                return Json("支付失败");
            }


            OrderInfo orderInfo = _unitOfWork.OrderInfoService.GetOrdersByOrderId(orderId);
            List<Order_Goods> orderGoods = orderInfo.Order_Goods.ToList();
            List<Goods> goodsList = new List<Goods>();

            //更改库存
            foreach (Order_Goods item in orderGoods)
            {
                Goods goods = _unitOfWork.GoodsService.GetGoods(item.goods_id);
                goods.goods_count -= item.goods_count;//更改库存
                goods.sell_count += item.goods_count;//增加销量
                _unitOfWork.GoodsService.EditEntity(goods);

                goodsList.Add(goods);

            }


            PayModel payModel = new PayModel
            {
                UserInfo = CurrentUser,
                OrderInfo = orderInfo,
                OrderGoodses = orderInfo.Order_Goods.ToList(),
                Goodses = goodsList

            };
            return View(payModel);
        }



    }
}