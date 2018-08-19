using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using BLL;
using BLLFactory;
using Common;
using CZBK.ItcastOA.Common;
using IBLL;
using Model;
using Newtonsoft.Json;
using WebUI.Models;

namespace WebUI.Controllers
{

    [RoutePrefix("user")]
    public class UserController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public FileStreamResult ValidateCode()
        {
            ValidateCode validateCode = new ValidateCode();
            string _validateCode = validateCode.CreateValidateCode(4);
            Session["validateCode"] = _validateCode;
            byte[] buf = validateCode.CreateValidateGraphic(_validateCode);
            return new FileStreamResult(new MemoryStream(buf), "image/jpeg");
        }

        [Route("login")]
        [HttpGet]
        public ActionResult Login()
        {
            UserInfo user = new UserInfo();
            if (Request.Cookies["loginNameCookie"] != null && Request.Cookies["loginPwdCookie"] != null)
            {
                string userName = Request.Cookies["loginNameCookie"].Value;
                string userPwd = Request.Cookies["loginPwdCookie"].Value;
                user = _unitOfWork.UserInfoService.CheckUser(userName, userPwd);
            }


            return View(user);
        }


        [Route("login")]
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (loginModel.ValidateCode != null)
            {
                if (!loginModel.ValidateCode.Equals(Session["validateCode"].ToString()))
                {
                    return Json(new { Msg = "验证码错误", Result = "no" });
                }


                UserInfo user = _unitOfWork.UserInfoService.CheckUser(loginModel.UserName, loginModel.UserPwd);

                if (user != null)
                {
                    LogHelper.Info("loginfo",
                        $"用户编号：{user.user_id}，用户名：{user.user_name}，操作描述：登录,ip="+Request.UserHostAddress);
                    if (loginModel.RemPwd)
                    {
                        user.remeber_pwd = (int?)Tool.IsRememberPwd.Yes;
                        _unitOfWork.UserInfoService.EditEntity(user);

                        HttpCookie nameCookie =
                            new HttpCookie("userName", user.user_name) { Expires = DateTime.Now.AddDays(7) };

                        HttpCookie pwdCookie =
                            new HttpCookie("userPwd", user.user_pwd) { Expires = DateTime.Now.AddDays(7) };

                        HttpCookie isRemPwdCookie =
                            new HttpCookie("isRemPwd", true.ToString()) { Expires = DateTime.Now.AddDays(7) };

                        HttpCookie loginNameCookie =
                            new HttpCookie("loginNameCookie", user.user_name) { Expires = DateTime.Now.AddDays(7), Path = "/user/login" };

                        HttpCookie loginPwdCookie =
                            new HttpCookie("loginPwdCookie", user.user_pwd) { Expires = DateTime.Now.AddDays(7), Path = "/user/login" };

                        HttpCookie loginIsRemPwdCookie =
                            new HttpCookie("loginIsRemPwdCookie", true.ToString()) { Expires = DateTime.Now.AddDays(7), Path = "/user/login" };


                        Response.Cookies.Add(nameCookie);
                        Response.Cookies.Add(pwdCookie);
                        Response.Cookies.Add(isRemPwdCookie);

                        Response.Cookies.Add(loginNameCookie);
                        Response.Cookies.Add(loginPwdCookie);
                        Response.Cookies.Add(loginIsRemPwdCookie);
                    }
                    else
                    {
                        user.remeber_pwd = (int?)Tool.IsRememberPwd.No;
                        _unitOfWork.UserInfoService.EditEntity(user);

                        if (Response.Cookies["userName"] != null &&
                            Response.Cookies["userPwd"] != null &&
                            Response.Cookies["loginNameCookie"] != null &&
                            Response.Cookies["isRemPwd"] != null &&
                            Response.Cookies["loginPwdCookie"] != null &&
                            Response.Cookies["loginIsRemPwdCookie"] != null
                            )
                        {
                            Response.Cookies["userName"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["userPwd"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["isRemPwd"].Expires = DateTime.Now.AddDays(-1);

                            Response.Cookies["loginNameCookie"].Path = "/user/login";
                            Response.Cookies["loginNameCookie"].Expires = DateTime.Now.AddDays(-1);

                            Response.Cookies["loginPwdCookie"].Path = "/user/login";
                            Response.Cookies["loginPwdCookie"].Expires = DateTime.Now.AddDays(-1);

                            Response.Cookies["loginIsRemPwdCookie"].Path = "/user/login";
                            Response.Cookies["loginIsRemPwdCookie"].Expires = DateTime.Now.AddDays(-1);
                        }
                    }

                    Session["UserInfo"] = user;
                    return Json("/home/index");
                }
                return Json(new { Msg = "登录失败，用户名或密码错误", Result = "no" });
            }

            return Json(new { Msg = "验证码不能为空", Result = "no" });

        }



        [HttpPost]
        [Route("register")]
        public ActionResult Register(string userName, string userPwd, string userTel, string userEmail)
        {
            if (userName != null && userPwd != null)
            {
                if (_unitOfWork.UserInfoService.GetUserInfoByName(userName)!=null)
                {
                    return Json("no");
                }
                UserInfo user = new UserInfo
                {
                    user_name = userName,
                    user_pwd = Tool.GetMd5(userPwd),
                    user_tel = userTel,
                    user_email = userEmail,
                    user_head_portrait = "default.jpg",
                    regist_time = DateTime.Now.ToLocalTime(),
                    remeber_pwd = (int?)Tool.IsRememberPwd.No,
                    receiver = userName,
                    user_flag = (int)Tool.UserFlag.Normal
                };

                try
                {
                    if (_unitOfWork.UserInfoService.AddEntity(user) != null)
                    {
                        LogHelper.Info("loginfo",
                            $"用户编号：{user.user_id}，用户名：{user.user_name}，操作描述：注册,ip="+Request.UserHostAddress);
                        Session["UserInfo"] = user;
                        return Json("/home/index");
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Error("logerror", "注册异常,用户名:" + user.user_name, e);
                }
               
            }



            return View("Login");
        }


        [User]
        public ActionResult Demo()
        {
            return View();
        }

        [User]
        [Route("shopcart")]
        [HttpGet]
        public ActionResult ShopCart()
        {
            //if (Request.IsAjaxRequest())
            //{
            //    if (Request["action"] != null)
            //    {
            //        string action = Request["action"];
            //        List<ShopCart_Goods> list = new List<ShopCart_Goods>();
            //        if (Request["ids"] != null)
            //        {
            //            int[] ids = JsonConvert.DeserializeObject<int[]>(Request["ids"]);
            //            foreach (int id in ids)
            //            {
            //                ShopCart_Goods shopCartGoods =
            //                    _unitOfWork.ShopCartGoodsService.GetShopCartGoods(id, CurrentUser.user_id);
            //                list.Add(shopCartGoods);
            //            }
            //        }

            //        switch (action)
            //        {
            //            case "delete":
            //                {

            //                    if (_unitOfWork.ShopCartGoodsService.DeleteEntity(list))
            //                    {
            //                        LogHelper.Info("loginfo",
            //                            $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：删除购物车商品，详情：编号-{list[0].goods_id}");
            //                        return Json("删除成功");
            //                    }

            //                    return Json("删除失败");
            //                }
            //            case "clearing":
            //                {
            //                    List<Goods> goodses = new List<Goods>();
            //                    List<Order_Goods> orderGoodses = new List<Order_Goods>();
            //                    OrderInfo orderInfo = new OrderInfo
            //                    {
            //                        order_id = DateTime.Now.Ticks.ToString(),
            //                        user_id = CurrentUser.user_id,
            //                        order_express_id = DateTime.Now.Millisecond.ToString(),
            //                        order_flag = (int?)Tool.OrderFlag.StayPay,

            //                        order_time = DateTime.Now.ToLocalTime(),
            //                        order_totalMoney = 0
            //                    };

            //                    foreach (ShopCart_Goods item in list)
            //                    {
            //                        Goods goods = AbstractFactory.GetGoodsService().GetGoods(item.goods_id);

            //                        //订单的所有商品
            //                        Order_Goods orderGoods = new Order_Goods
            //                        {
            //                            order_id = orderInfo.order_id,
            //                            goods_id = goods.goods_id,
            //                            goods_count = item.goods_count,
            //                            goods_money = item.goods_count * goods.goods_price
            //                        };
            //                        orderGoodses.Add(orderGoods);
            //                        orderInfo.order_totalMoney += orderGoods.goods_money;
            //                    }

            //                    orderInfo.Order_Goods = orderGoodses;//订单的商品

            //                    if (_unitOfWork.OrderInfoService.AddEntity(orderInfo) != null)
            //                    {
            //                        if (_unitOfWork.ShopCartGoodsService.DeleteShopCartGoods(list))
            //                        {
            //                            //SessionHelper.SetSession("orderInfoId", orderInfo.order_id);
            //                            LogHelper.Info("loginfo",
            //                                $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：结算，详情：订单编号-{orderInfo.order_id}");
            //                            return Json("/goods/pay/" + orderInfo.order_id);
            //                        }
            //                    }
            //                    break;
            //                }

            //        }
            //    }
            //}


            return View(GetUserShopCart());
        }


        [User]
        [Route("shopcart")]
        [HttpPost]
        public ActionResult ShopCart(string action, string ids)
        {


            //string action = Request["action"];
            List<ShopCart_Goods> list = new List<ShopCart_Goods>();

            int[] idsAarry = JsonConvert.DeserializeObject<int[]>(Request["ids"]);
            foreach (int id in idsAarry)
            {
                ShopCart_Goods shopCartGoods =
                    _unitOfWork.ShopCartGoodsService.GetShopCartGoods(id, CurrentUser.user_id);
                list.Add(shopCartGoods);
            }

            switch (action)
            {
                case "delete":
                    {

                        if (_unitOfWork.ShopCartGoodsService.DeleteEntity(list))
                        {
                            return Json("删除成功");
                        }

                        return Json("删除失败");
                    }
                case "clearing":
                    {
                        
                        List<Goods> goodses = new List<Goods>();
                        List<Order_Goods> orderGoodses = new List<Order_Goods>();
                        OrderInfo orderInfo = new OrderInfo
                        {
                            order_id = DateTime.Now.Ticks.ToString(),
                            user_id = CurrentUser.user_id,
                            order_flag = (int?)Tool.OrderFlag.StayPay,

                            order_time = DateTime.Now.ToLocalTime(),
                            order_totalMoney = 0
                        };

                        foreach (ShopCart_Goods item in list)
                        {
                            Goods goods = AbstractFactory.GetGoodsService().GetGoods(item.goods_id);

                            //订单的所有商品
                            Order_Goods orderGoods = new Order_Goods
                            {
                                order_id = orderInfo.order_id,
                                goods_id = goods.goods_id,
                                goods_count = item.goods_count,
                                goods_money = item.goods_count * goods.goods_price
                            };
                            orderGoodses.Add(orderGoods);
                            orderInfo.order_totalMoney += orderGoods.goods_money;
                        }

                        orderInfo.Order_Goods = orderGoodses;//订单的商品

                        if (AbstractFactory.GetOrderInfoService().AddEntity(orderInfo) != null)
                        {
                            if (_unitOfWork.ShopCartGoodsService.DeleteShopCartGoods(list))
                            {
                                //SessionHelper.SetSession("orderInfoId", orderInfo.order_id);

                                return Json("/goods/pay/" + orderInfo.order_id);
                            }
                        }
                        break;
                    }

            }




            return View(GetUserShopCart());
        }

        /// <summary>
        /// 获取当前用户的购物车数据
        /// </summary>
        /// <returns></returns>
        private ShopCartModel GetUserShopCart()
        {
            int sumPrice = 0;
            int sumCount = 0;

            var shopCart = _unitOfWork.ShopCartGoodsService.GetShopCartGoods(CurrentUser.user_id).ToList();
            List<ShopCartGoodsModel> goodsModels = new List<ShopCartGoodsModel>();

            foreach (var t in shopCart)
            {
                Goods item = _unitOfWork.GoodsService.GetGoods(t.goods_id);

                sumCount += t.goods_count;
                sumPrice += item.goods_price * t.goods_count;
                ShopCartGoodsModel goodsModel = new ShopCartGoodsModel
                {
                    ShopCartGoodsCount = t.goods_count,
                    TotalMoney = t.goods_count * item.goods_price,
                    UserIsCollectGoods = _unitOfWork.CollectGoodsService.UserIsCollectGoods(CurrentUser, item.goods_id),
                    GoodsAddTime = item.goods_addTime,
                    GoodsFlag = item.goods_flag,
                    GoodsCount = item.goods_count,
                    GoodsId = item.goods_id,
                    GoodsInfo = item.goods_info,
                    GoodsName = item.goods_name,
                    GoodsPictureAdress = item.Picture.FirstOrDefault().picture_adress,
                    GoodsPrice = item.goods_price,
                    GoodsTitle = item.goods_title,
                    GoodsTypeId = item.goods_type_id
                };
                goodsModels.Add(goodsModel);
            }

            ShopCartModel shopCartModel = new ShopCartModel
            {

                GoodsModels = goodsModels,
                SumGoodsCount = sumCount,
                SumPrice = sumPrice
            };
            return shopCartModel;
        }

        /// <summary>
        /// 购物车操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [Route("shopcartpartialview")]
        [HttpPost]
        public ActionResult ShopCartPartialView(string action, int goodsId)
        {
            if (CurrentUser != null)
            {
                ShopCart_Goods shopCartGoods;
                switch (action)
                {
                    case "up":

                        shopCartGoods = _unitOfWork.ShopCartGoodsService.GetShopCartGoods(goodsId, CurrentUser.user_id);
                        if (shopCartGoods != null)
                        {
                            //购物车数量
                            shopCartGoods.goods_count = shopCartGoods.goods_count + 1 > 100 ? 100 : shopCartGoods.goods_count + 1;
                            if (_unitOfWork.GoodsService.GetGoods(goodsId).goods_count < shopCartGoods.goods_count)
                            {
                                return Json("商品库存不足");
                            }
                            _unitOfWork.ShopCartGoodsService.EditEntity(shopCartGoods);
                        }

                        break;
                    case "down":
                        shopCartGoods = _unitOfWork.ShopCartGoodsService.GetShopCartGoods(goodsId, CurrentUser.user_id);
                        if (shopCartGoods != null)
                        {
                            shopCartGoods.goods_count = shopCartGoods.goods_count - 1 < 0 ? 1 : shopCartGoods.goods_count - 1;
                            _unitOfWork.ShopCartGoodsService.EditEntity(shopCartGoods);
                        }
                        break;
                    case "remove":
                        shopCartGoods = _unitOfWork.ShopCartGoodsService.GetShopCartGoods(goodsId, CurrentUser.user_id);
                        if (shopCartGoods != null)
                        {
                            try
                            {
                                if (_unitOfWork.ShopCartGoodsService.DeleteEntity(shopCartGoods))
                                {
                                    LogHelper.Info("loginfo",
                                        $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：移除购物车商品，详情：商品编号{shopCartGoods.goods_id}");
                                }
                            }
                            catch (Exception e)
                            {
                                LogHelper.Error("logerror", "移除购物车商品异常", e);
                            }

                        }
                        break;
                }
                return Json(GetUserShopCart());
            }

            return Json("no");

        }

        [User]
        [Route("collect")]
        public ActionResult Collect(int page = 1)
        {
            page = page < 1 ? 1 : page;

            List<CollectGoods> collectGoodses = _unitOfWork.CollectGoodsService.LoadPageEntities(page, Setting.PageSize,
                out int totalCount, m => m.collect_id.Equals(CurrentUser.user_id), m => m.goods_id, true).ToList();

            List<GoodsModel> goodsModels = new List<GoodsModel>();

            foreach (CollectGoods item in collectGoodses)
            {
                GoodsModel goodsModel = new GoodsModel
                {
                    Goods = _unitOfWork.GoodsService.GetGoods(item.goods_id),
                    UserIsCollectGoods = _unitOfWork.CollectGoodsService.UserIsCollectGoods(CurrentUser, item.goods_id)
                };
                goodsModels.Add(goodsModel);

            }

            ViewData["pageIndex"] = page;
            ViewData["allPage"] = (int)Math.Ceiling(totalCount / (Setting.PageSize * 1.0));

            return View(goodsModels);
        }

        [User]
        [Route("info")]
        [HttpGet]
        public ActionResult PersonalCenter()
        {
            UserInfo user = CurrentUser;

            return View(user);
        }


        [User]
        [Route("upload")]
        [HttpPost]
        public ActionResult UploadUserHeadPic()
        {
            UserInfo user = CurrentUser;
            if (Request.Files.Count > 0)
            {
                if (user.user_head_portrait != "default.jpg")
                {
                    PictureHelper.DeltePicture(Server.MapPath("./.."), user.user_head_portrait);//删除原来的头像
                }
                try
                {
                    HttpPostedFileBase file = Request.Files[0];

                    string currentPath = Server.MapPath("./..") + Setting.HeadPicFolder;
                    Directory.CreateDirectory(currentPath);
                    string newName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                    file.SaveAs(currentPath + newName);
                    user = _unitOfWork.UserInfoService.LoadEntities(m => m.user_id == CurrentUser.user_id).FirstOrDefault();
                    user.user_head_portrait = newName;

                    Session["UserInfo"] = user;//刷新头像


                    _unitOfWork.UserInfoService.EditEntity(user);
                }
                catch (Exception e)
                {
                    LogHelper.Error("logerror", "更换头像异常", e);
                }


            }

            return RedirectToAction("PersonalCenter");
        }


        [User]
        [Route("info")]
        [HttpPost]
        public ActionResult PersonalCenter(UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                UserInfo user = _unitOfWork.UserInfoService.GetUserInfoById(userInfo.user_id);
                user.user_adress = userInfo.user_adress;
                user.user_name = userInfo.user_name;
                user.user_email = userInfo.user_email;
                user.user_sex = userInfo.user_sex;
                user.user_tel = userInfo.user_tel;

                if (_unitOfWork.UserInfoService.EditEntity(user))
                {
                    Session["UserInfo"] = user;
                    return View(CurrentUser);

                }
            }
            return View(CurrentUser);
        }

        /// <summary>
        /// 查找订单
        /// </summary>
        /// <param name="orderInfos">用户订单数据</param>
        /// <returns></returns>
        private List<OrderInfoModel> GetOrderInfo(List<OrderInfo> orderInfos)
        {
            List<OrderInfoModel> orderInfoModels = new List<OrderInfoModel>();
            foreach (var item in orderInfos)
            {
                OrderInfoModel orderInfoModel = new OrderInfoModel
                {
                    UserId = item.user_id,
                    OrderGoods = item.Order_Goods,
                    OrderExpressId = item.order_express_id,
                    OrderFlag = Tool.OrderFlagToString((Tool.OrderFlag)item.order_flag),
                    OrderId = item.order_id,
                    OrderTime = item.order_time,
                    OrderTotalMoney = item.order_totalMoney
                };
                orderInfoModels.Add(orderInfoModel);
            }

            return orderInfoModels;

        }




        [User]
        [Route("purchasehistory")]
        public ActionResult PurchaseHistory()
        {
            List<OrderInfo> orderInfos = _unitOfWork.OrderInfoService.GetOrdersListByUserId(CurrentUser.user_id, (int)Tool.OrderFlag.Received);
            ViewBag.user = CurrentUser;
            return View(GetOrderInfo(orderInfos));
        }

        /// <summary>
        /// 待支付
        /// </summary>
        /// <returns></returns>
        [User]
        [Route("staypay")]
        [HttpGet]
        public ActionResult StayPay()
        {
            List<OrderInfo> orderInfos = _unitOfWork.OrderInfoService.GetOrdersListByUserId(CurrentUser.user_id, (int)Tool.OrderFlag.StayPay);
            ViewBag.user = CurrentUser;
            return View(GetOrderInfo(orderInfos));

        }

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <returns></returns>
        [User]
        [Route("staypay")]
        [HttpPost]
        public ActionResult StayPay(string orderId)
        {
            OrderInfo orderInfo = _unitOfWork.OrderInfoService.GetOrdersByOrderId(orderId);
           

            if (orderInfo.order_flag==(int)Tool.OrderFlag.StayPay)
            {
                orderInfo.order_flag = (int?)Tool.OrderFlag.StaySendOut;
                _unitOfWork.OrderInfoService.EditEntity(orderInfo);
                LogHelper.Info("loginfo",
                    $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：付款，详情：订单编号{orderInfo.order_id}");
            }
           
            return RedirectToAction("StaySendOut", "User");
        }


        /// <summary>
        /// 待发货
        /// </summary>
        /// <returns></returns>
        [User]
        [Route("staysendout")]
        public ActionResult StaySendOut()
        {

            List<OrderInfo> orderInfos = _unitOfWork.OrderInfoService.GetOrdersListByUserId(CurrentUser.user_id, (int)Tool.OrderFlag.StaySendOut);
            ViewBag.user = CurrentUser;
            return View(GetOrderInfo(orderInfos));
        }

        /// <summary>
        /// 待收货
        /// </summary>
        /// <returns></returns>
        [User]
        [Route("stayreceiving")]
        [HttpGet]
        public ActionResult StayReceiving()
        {
            List<OrderInfo> orderInfos = _unitOfWork.OrderInfoService.GetOrdersListByUserId(CurrentUser.user_id, (int)Tool.OrderFlag.StayReceiving);
            ViewBag.user = CurrentUser;
            return View(GetOrderInfo(orderInfos));
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        [User]
        [Route("stayreceiving")]
        [HttpPost]
        public ActionResult StayReceiving(string orderId)
        {
            OrderInfo orderInfo = _unitOfWork.OrderInfoService.GetOrdersByOrderId(orderId);
            orderInfo.order_flag = (int?)Tool.OrderFlag.StayComment;
            _unitOfWork.OrderInfoService.EditEntity(orderInfo);
            LogHelper.Info("loginfo",
                $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：确认收货，详情：订单编号{orderInfo.order_id}");
            return RedirectToAction("StayComment", "User");
        }

        /// <summary>
        /// 待评论
        /// </summary>
        /// <returns></returns>
        [User]
        [HttpGet]
        [Route("staycomment")]
        public ActionResult StayComment()
        {
            ViewBag.user = CurrentUser;
            return View(GetOrderInfo(GetOrderinfoNotComment()));
        }

        /// <summary>
        /// 修改收货信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ActionResult EditReciverInfo(UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                UserInfo user = _unitOfWork.UserInfoService.GetUserInfoById(userInfo.user_id);
                user.receiver = userInfo.receiver;
                user.user_adress = userInfo.user_adress;
                user.user_tel = userInfo.user_tel;
                if (_unitOfWork.UserInfoService.EditEntity(user))
                {
                    Session["UserInfo"] = user;
                    return Json(new { Msg = "ok", User = new { user.receiver, user.user_adress, user.user_tel } });
                }


            }

            return Json(new { Msg = "no" });
        }

        /// <summary>
        /// 评论商品
        /// </summary>
        /// <returns></returns>
        [User]
        [HttpPost]
        [Route("staycomment")]
        public ActionResult StayComment(string orderId, int goodsId, string comment)
        {
            OrderInfo orderInfo = _unitOfWork.OrderInfoService.GetOrdersByOrderId(orderId);

            Order_Goods orderGoods = orderInfo.Order_Goods.Single(m => m.goods_id.Equals(goodsId));

            orderGoods.is_commect = (int)Tool.IsComment.Yes;

            //将商品评论插入到评论表
            Comment goodsComment = new Comment
            {
                comment_content = comment,
                comment_flag = (int)Tool.CommentFlag.Normal,
                comment_time = DateTime.Now.ToLocalTime(),
                goods_id = orderGoods.goods_id,
                user_id = CurrentUser.user_id
            };
            _unitOfWork.CommentService.AddEntity(goodsComment);
            _unitOfWork.OrderGoodsService.EditEntity(orderGoods);

            ViewBag.user = CurrentUser;
            return View(GetOrderInfo(GetOrderinfoNotComment()));
        }

        [HttpPost]
        public ActionResult DeleteOrder(string orderId)
        {
            OrderInfo orderInfo = _unitOfWork.OrderInfoService.GetOrdersByOrderId(orderId);
            _unitOfWork.OrderInfoService.DeleteEntity(orderInfo);
            LogHelper.Info("loginfo",
                $"用户编号：{CurrentUser.user_id}，用户名：{CurrentUser.user_name}，操作描述：删除订单，详情：订单编号{orderInfo.order_id}");
            return RedirectToAction("PurchaseHistory", "User");
        }

        /// <summary>
        /// 获取没有评论的订单
        /// </summary>
        /// <returns></returns>
        private List<OrderInfo> GetOrderinfoNotComment()
        {
            List<OrderInfo> orderInfos = _unitOfWork.OrderInfoService.GetOrdersListByUserId(CurrentUser.user_id, (int)Tool.OrderFlag.StayComment);
            //如果该订单下的所有商品都评论移除该订单 不能使用foreach否则  foreach 语句是对枚举数的包装，它只允许从集合中读取，不允许写入集合。
            //也就是,不能在foreach里遍历的时侯把它的元素进行删除或增加的操作的



            for (int i = 0; i < orderInfos.Count; i++)
            {
                var item = orderInfos[i];
                if (item.Order_Goods.All(m => m.is_commect == (int)Tool.IsComment.Yes))
                {
                    item.order_flag = (int?)Tool.OrderFlag.Received;
                    _unitOfWork.OrderInfoService.EditEntity(item);
                    orderInfos.Remove(item);
                }
            }

            return orderInfos;
        }


    }



    /// <summary>
    /// 标记，判断用户是否登录了,限制部分网页只能在登录后操作
    /// </summary>
    public class UserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            BaseController baseController = new BaseController();
            HttpContextBase context = filterContext.HttpContext;
            if (baseController.CurrentUser == null)
            {
                //context.Server.Execute("/User/Login");
                context.Response.Redirect("/user/login");
            }

        }
    }
}