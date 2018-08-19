using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Model;
using BLLFactory;
using Common;
using IBLL;
using log4net;
using WebUI.Models;

namespace WebUI.Controllers
{

    [RoutePrefix("home")]
    public class HomeController : BaseController
    {
        /*
         * 刘东明
         * 18071796804
         * qq 3262809910
         */
        // GET: /Home



        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [Route("exit")]
        public ActionResult Exit()
        {
            if (Response.Cookies["userName"] != null && Response.Cookies["userPwd"] != null && Response.Cookies["isRemPwd"] != null)
            {
                Response.Cookies["userName"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["userPwd"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["isRemPwd"].Expires = DateTime.Now.AddDays(-1);
                Session["UserInfo"] = null;
            }

            return RedirectToAction("Index");
        }


        public ActionResult Index()
        {
            var tempGoodsList = _unitOfWork.GoodsService.GetGoodsList();
            IEnumerable<GoodsType> tempgoodsTypesTypes = _unitOfWork.GoodsTypeService.LoadEntities(m => true);

            List<IndexModel> indexModels = new List<IndexModel>();

            foreach (var item in tempgoodsTypesTypes)
            {
                IndexModel indexModel = new IndexModel
                {
                    GoodsType = item
                };
                var goods = tempGoodsList.Where(m => m.goods_type_id == item.goods_type_id);
                List<GoodsModel> goodsModels = new List<GoodsModel>();

                //查看用户是否收藏了此商品
                foreach (var g in goods)
                {
                    var goodsModel = new GoodsModel
                    {
                        Goods = g,
                        UserIsCollectGoods = _unitOfWork.CollectGoodsService.UserIsCollectGoods(CurrentUser, g.goods_id)
                    };
                    goodsModels.Add(goodsModel);
                }

                indexModel.Goodses = goodsModels;
                indexModels.Add(indexModel);
            }



            //最畅销的商品
            var temp = tempGoodsList.OrderByDescending(m => m.sell_count).ToList();
            temp = temp.Count >= 7 ? temp.GetRange(0, 7) : temp;
            var bestSellGoods = new List<GoodsModel>();
            foreach (var itemModel in temp)
            {
                bestSellGoods.Add(new GoodsModel
                {
                    Goods = itemModel,
                    UserIsCollectGoods = _unitOfWork.CollectGoodsService.UserIsCollectGoods(CurrentUser, itemModel.goods_id)
                });
            }

            ViewData["goods"] = bestSellGoods;
            //ViewBag.wheel = _unitOfWork.GoodsService.GetGoodsListByTypeName("车轮").GetRange(0, 2);
            //ViewBag.engineOil = _unitOfWork.GoodsService.GetGoodsListByTypeName("机油").GetRange(0, 3);

            var tempComments = _unitOfWork.CommentService.GetComments().OrderByDescending(m => m.comment_time).ToList();
            ViewData["comments"] = tempComments.Count >= 8 ? tempComments.GetRange(0, 8) : tempComments;

            return View(indexModels);
        }

        [Route("search")]
        public ActionResult Search(string goodsName)
        {
            List<Goods> goodses = _unitOfWork.GoodsService.GetGoods(goodsName);
            List<GoodsModel> goodsModels = new List<GoodsModel>();

            foreach (var item in goodses)
            {
                GoodsModel goodsModel = new GoodsModel
                {
                    Goods = item,
                    UserIsCollectGoods = _unitOfWork.CollectGoodsService.UserIsCollectGoods(CurrentUser, item.goods_id)
                };
                goodsModels.Add(goodsModel);
            }

            SearchModel searchModel = new SearchModel
            {
                GoodsModels = goodsModels,
                GoodsName = goodsName
            };
            return View(searchModel);
        }


        private List<Goods> GetGoodsListByType(int typeId)
        {
            List<Goods> list = new List<Goods>();

            List<Goods> temp = _unitOfWork.GoodsService.LoadEntities(m => m.goods_type_id == typeId).ToList();
            for (int i = 0; i < 4; i++)
            {
                Goods goods = temp[i];
                goods.Picture = _unitOfWork.PictureService.LoadEntities(m => m.goods_id == goods.goods_id).ToList();

                list.Add(goods);

            }

            return list;
        }



    }
}