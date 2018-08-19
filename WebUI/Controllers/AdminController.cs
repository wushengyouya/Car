using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using BLLFactory;
using Common;
using IBLL;
using Model;
using Newtonsoft.Json;

namespace WebUI.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //登录
        [Route("login")]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            if (_unitOfWork.AdminService.CheckAdmin(admin))
            {
                Session["Admin"] = admin.admin_name;
                LogHelper.Info("loginfo",
                    $"管理员：{admin.admin_name}，操作描述：登录，详情：登录ip-{Request.UserHostAddress}");
            }

            
            return RedirectToAction("AllGoods","GoodsManager");
        }
        #region 手风琴控制-设置主题
        /// <summary>
        /// 控制手风琴菜单
        /// </summary>
        /// <param name="href"></param> 
        /// <param name="index"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public ContentResult SetList(string href, string index, string text)
        {
            Session["setList"] = href;
            Session["index"] = index;
            Session["text"] = text;//顶部标题


            return Content("列表激活" + href);
        }

        /// <summary>
        /// 设置主题
        /// </summary>
        /// <param name="themeColor"></param>
        /// <param name="mainColor"></param>
        /// <param name="accentColor"></param>
        /// <returns></returns>
        public ContentResult SetTheme(string themeColor, string mainColor, string accentColor)
        {
            Session["themeColor"] = themeColor;
            Session["mainColor"] = mainColor;
            Session["accentColor"] = accentColor;

            return Content("主题设置成功");
        }
        #endregion


    }
}