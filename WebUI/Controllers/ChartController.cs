using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLLFactory;
using Common;
using Model;

namespace WebUI.Controllers
{
    [RoutePrefix("chart")]
    [Admin]
    public class ChartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //当日销售
        [Route("today")]
        public ActionResult TodaySalesChart()
        {
            return View();
        }

        //销售统计
        [Route("all")]
        public ActionResult AllSalesChart()
        {
            if (Request.IsAjaxRequest())
            {
                List<GoodsType> goodsTypes = new List<GoodsType>();
                var goods = _unitOfWork.OrderGoodsService.GetOrderGoodses();
                foreach (var item in goods)
                {
                    goodsTypes.Add(item.Goods.GoodsType);
                }

                IEnumerable<IGrouping<string, GoodsType>> groupBy =
                    goodsTypes.GroupBy(m => m.goods_type_name);

              List<Temp> list = new List<Temp>();
                foreach (var item in groupBy)
                {

                    list.Add(new Temp()
                    {
                        Data = float.Parse((item.Count() / (goodsTypes.Count * 1.0)).ToString("f3")) * 100,
                        Name = item.Key
                    });

                }

                
                return Json(list);
            }
           
            return View();
        }

      
        //评论统计
        [Route("comment")]
        public ActionResult CommentChart()
        {
            return View();
        }
    }

    public class Temp
    {
        public string Name { get; set; }
        public float Data { get; set; }
    }
}