using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLLFactory;
using Common;
using IBLL;
using Model;
using Newtonsoft.Json;

namespace WebUI.Controllers
{
    [RoutePrefix("goodsmanager")]
    [Admin]
    public class GoodsManagerController : Controller
    {
        private readonly IGoodsTypeService _goodsTypeService;
        private readonly IGoodsService _goodsService;
        private readonly IPictureService _pictureService;

        public GoodsManagerController(IGoodsTypeService goodsTypeService, IGoodsService goodsService, IPictureService pictureService)
        {
            _goodsTypeService = goodsTypeService;
            _goodsService = goodsService;
            _pictureService = pictureService;
        }
        //商品类别
        [Route("type")]
        public ActionResult GoodsType()
        {
            if (Request.IsAjaxRequest())
            {
                string action = Request["action"];
                string msg = "no";
                switch (action)
                {
                    case "add":
                        {
                            string goodsTypeName = Request["goodsTypeName"];
                            if (_goodsTypeService.GetGoodsType(goodsTypeName) != null)
                            {
                                return Json(new { Msg = "had" });
                            }
                            GoodsType goodsType = new GoodsType { goods_type_name = goodsTypeName, goods_type_addTime = DateTime.Now.ToLocalTime() };
                            try
                            {
                                if (_goodsTypeService.AddEntity(goodsType) != null)
                                {
                                    //操作结果
                                    msg = "ok";
                                }
                            }
                            catch (Exception e)
                            {
                                LogHelper.Error("logerror", "添加商品类别异常,类别名:" + goodsType.goods_type_name, e);
                            }


                            break;
                        }
                    case "delete":
                        int[] ids = JsonConvert.DeserializeObject<int[]>(Request["ids"]);
                        List<GoodsType> list = new List<GoodsType>();
                        foreach (int item in ids)
                        {
                            GoodsType goodsType = new GoodsType() { goods_type_id = item };
                            list.Add(goodsType);
                        }

                        if (_goodsTypeService.DeleteEntity(list))
                        {
                            msg = "ok";
                        }

                        break;
                    case "edit":
                        {


                            GoodsType goodsType = _goodsTypeService.GetGoodsType(int.Parse(Request["goodsId"]));
                            goodsType.goods_type_name = Request["newGoodsTypeName"];


                            try
                            {
                                _goodsTypeService.EditEntity(goodsType);
                                msg = "ok";
                            }
                            catch (Exception e)
                            {
                                LogHelper.Error("logerror", "更新商品类别异常,类别编号:" + goodsType.goods_type_id, e);
                            }

                            break;
                        }
                }

                //筛选数据,避免报错
                var jsonResult = _goodsTypeService.LoadEntities(m => true).Select(m => new { m.goods_type_id, m.goods_type_name, m.goods_type_addTime });
                return Json(data: new { Msg = msg, JsonData = jsonResult });
            }

            //强类型视图
            ViewModel viewModel = new ViewModel
            {
                GoodsTypeList = _goodsTypeService.LoadEntities(m => true).ToList()
            };
            return View(viewModel);
        }

        //所有商品
        //m.goods_id,
        //m.goods_type_id,
        //m.goods_name,
        //m.goods_price,
        //m.goods_flag,
        //m.goods_info,
        //m.goods_count,
        //m.goods_addTime
        //所有商品
        [Route("all")]
        public ActionResult AllGoods()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");//当前页码

            var data = _goodsService.LoadPageEntities(pageIndex: pageIndex, pageSize: Setting.PageSize, totalCount: out var totalCount, whereLambda: m => true, orderByLambda: m => m.goods_addTime, isAsc: false);

            ViewData["totalCount"] = Math.Ceiling(totalCount / (Setting.PageSize * 1.0));//最大页数

            if (Request.IsAjaxRequest())
            {
                string action = Request["action"];
                string msg = "no";
                switch (action)
                {
                    case "add":
                        //是否有同名商品
                        if (_goodsService.GetGoods(Request["goods_name"]).Count == 1)
                        {
                            return Json(new { Msg = "had" });
                        }
                        Goods goods = new Goods
                        {
                            goods_type_id = int.Parse(Request["goods_type_id"]),
                            goods_name = Request["goods_name"],
                            goods_price = int.Parse(Request["goods_price"]),
                            goods_flag = int.Parse(Request["goods_flag"]),
                            goods_count = int.Parse(Request["goods_count"]),
                            goods_info = Request["goods_info"],
                            goods_title = Request["goods_title"],
                            goods_addTime = DateTime.Now.ToLocalTime(),
                            sell_count = 0
                        };
                        try
                        {
                            if (_goodsService.AddEntity(goods) != null)
                            {
                                msg = "ok";
                                return Json(Url.Action("GoodsManagerDetail", "GoodsManager", new { id = goods.goods_id }));
                            }
                        }
                        catch (Exception e)
                        {
                            LogHelper.Error("logerror", "添加商品异常,名称:" + goods.goods_name, e);
                        }
                        break;
                    case "delete":
                        int[] ids = JsonConvert.DeserializeObject<int[]>(Request["ids"]);
                        List<Goods> list = new List<Goods>();

                        //商品的所有图片
                        List<string> goodsPicNameList = new List<string>();
                        foreach (int id in ids)
                        {
                            Goods tempGoods = _goodsService.GetGoods(id);
                            if (tempGoods == null)
                            {
                                return Json(new { Msg = "no" });
                            }
                            var pics = tempGoods.Picture.ToList();
                            list.Add(tempGoods);
                            foreach (var item in pics)
                            {
                                goodsPicNameList.Add(item.picture_adress);
                            }
                        }
                        if (_goodsService.DeleteEntity(list))
                        {
                            //删除硬盘上的商品图片
                            if (PictureHelper.DeltePicture(Server.MapPath("./.."), goodsPicNameList))
                            {
                                msg = "ok";
                            }
                        }

                        break;
                    case "edit":

                        goods = _goodsService.GetGoods(int.Parse(Request["goods_id"]));
                        goods.goods_type_id = int.Parse(Request["goods_type_id"]);
                        goods.goods_name = Request["goods_name"];
                        goods.goods_price = int.Parse(Request["goods_price"]);
                        goods.goods_flag = int.Parse(Request["goods_flag"]);
                        goods.goods_count = int.Parse(Request["goods_count"]);
                        goods.goods_info = Request["goods_info"];
                        goods.goods_title = Request["goods_title"];

                        if (_goodsService.EditEntity(goods))
                        {
                            msg = "ok";
                        }
                        break;
                    case "page":
                        return Json(data.Select(m => new
                        {
                            m.goods_id,
                            m.GoodsType.goods_type_name,
                            m.goods_name,
                            m.goods_price,
                            m.goods_flag,
                            m.goods_info,
                            m.goods_count,
                            m.goods_addTime,
                            m.goods_title

                        }));
                }

                var jsonResult = _goodsService.LoadEntities(m => true).Select(m => new
                {
                    m.goods_id,
                    m.GoodsType.goods_type_name,
                    m.goods_name,
                    m.goods_price,
                    m.goods_flag,
                    m.goods_info,
                    m.goods_count,
                    m.goods_addTime,
                    m.goods_title

                });
                return Json(new { Msg = msg, JsonData = jsonResult });
            }

            //var data = goodsService.LoadEntities(m => true);
            ViewModel viewModel = new ViewModel
            {
                GoodsList = data.ToList(),
                GoodsTypeList = AbstractFactory.GetGoodsTypeService().LoadEntities(m => true).ToList()
            };
            return View(viewModel);
        }

        //商品详情
        [Route("detail/{id}")]
        [HttpGet]
        public ActionResult GoodsManagerDetail(int id)
        {
            Session["text"] = "商品详情";
            Goods goods = AbstractFactory.GetGoodsService().GetGoods(id);

            ViewModel viewModel = new ViewModel
            {
                GoodsTypeList = _goodsTypeService.LoadEntities(m => true).ToList()

            };

            List<Picture> picturesList = _pictureService.GetGoodsPictures(id);


            //dynamic关系词标记变量为动态类型。dynamic的变量赋值操作，编译器不会进行类型检查。
            dynamic dy = new ExpandoObject();
            dy.viewModel = viewModel;
            dy.goods = goods;
            dy.picturesList = picturesList;


            return View(dy);
        }

        /// <summary>
        /// 上传商品图片
        /// </summary>
        public ActionResult GoodsPicUpLoad()
        {
            var httpFileCollection = Request.Files;
            if (httpFileCollection.Count == 0)
            {
                return Json(new { Msg = "no" });
            }

            HttpPostedFileBase file = Request.Files[0];

            var currentPath = Server.MapPath("./..") + Setting.GoodsPicFolder;
            var savePath = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";


            Directory.CreateDirectory(currentPath + savePath);

            if (file != null)
            {
                string newName = savePath + Guid.NewGuid() + Path.GetExtension(file.FileName);
                int goodsId = int.Parse(Request["id"]);
                file.SaveAs(currentPath + newName);
                Picture pic = new Picture
                {
                    goods_id = goodsId,
                    picture_adress = newName
                };
                if (_pictureService.AddEntity(pic) != null)
                {

                    return Json(new { Msg = "ok", JsonData = _pictureService.GetGoodsPictures(goodsId).Select(m => new { m.picture_id, m.picture_adress }) });
                }

            }

            return Json(new { Msg = "no" });
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="action"></param>
        /// <param name="picture"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GoodsManagerDetail(string action, Picture picture)
        {
            //Picture picture = new Picture
            //{
            //    picture_id = int.Parse(Request["picture_id"]),
            //    goods_id = goods_id,
            //    picture_adress = Request["picture_adress"]
            //};
            if ((picture = _pictureService.GetPicture(picture.picture_id)) != null)
            {

                if (PictureHelper.DeltePicture(Server.MapPath("./.."), new List<string> { picture.picture_adress }))
                {
                    if (_pictureService.DeleteEntity(picture))
                    {
                        // var jsonResult = _pictureService.LoadEntities(m => m.goods_id == picture.goods_id).Select(m => new { m.picture_id, m.goods_id, m.picture_adress });

                        return Json(new { Msg = "删除成功" });
                    }
                }

            }



            return Json(new { Msg = "删除失败", Pic = picture });

        }

        //添加商品
        [Route("add")]
        public ActionResult AddGoods()
        {
            return View();
        }

        //搜索商品
        [Route("search")]
        public ActionResult SearchGoods(string keyword)
        {
            Session["text"] = "搜索商品";
            List<Goods> goodses = _goodsService.GetGoods(keyword);
            List<GoodsType> goodsType = _goodsTypeService.GetGoodsTypeList();

            ViewBag.currentType = goodsType.FirstOrDefault().goods_type_name;
            ViewData["types"] = goodsType;
            return View(goodses);
        }

        [Route("searchtype")]
        public ActionResult SearchGoodsByType(string keyword)
        {
            Session["text"] = "搜索商品";
            List<Goods> goodses = _goodsService.GetGoodsListByTypeName(keyword);
            ViewData["types"] = _goodsTypeService.GetGoodsTypeList();
            ViewBag.currentType = keyword;
            return View("SearchGoods", goodses);
        }



    }
}