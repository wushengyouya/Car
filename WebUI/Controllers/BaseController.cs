using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLLFactory;
using Common;
using IBLL;
using Model;

namespace WebUI.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUserInfoService _userInfoService;

        public BaseController() { }
        public BaseController(IUnitOfWork unitOfWork)
        {
            _userInfoService = unitOfWork.UserInfoService;
            Setting.GoodsTypes = unitOfWork.GoodsTypeService.GetGoodsTypeList();
            Setting.UserInfo = CurrentUser;
            Setting.Goodses = unitOfWork.GoodsService.GetGoodsList().OrderByDescending(m => m.sell_count).Take(10);
        }
       
        public UserInfo CurrentUser
        {
            get
            {
                if (SessionHelper.GetSession("UserInfo")!=null)
                {
                    return SessionHelper.GetSession("UserInfo") as UserInfo;
                }
                if (CookieHelper.GetUserInfoCookie() != null)
                {
                    var user = CookieHelper.GetUserInfoCookie();
                    SessionHelper.Add("UserInfo", user);
                    return user;
                  
                }
                return null;

            }
          
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
          
            if (filterContext.HttpContext.Session["UserInfo"] == null)
            {
                SetSession(filterContext.HttpContext);
            }

        }

        private void SetSession(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies["userName"] != null && Request.Cookies["userPwd"] != null)
            {
                string userName = httpContext.Request.Cookies["userName"].Value;
                string userPwd = httpContext.Request.Cookies["userPwd"].Value;
                UserInfo userInfo = _userInfoService.CheckUser(userName, userPwd);
                if (userInfo != null)
                {
                   
                    SessionHelper.Add("UserInfo", userInfo);
                }
                else
                {
                    RedirectToAction("Login", "User");
                }
            }
            

        }
    }
}