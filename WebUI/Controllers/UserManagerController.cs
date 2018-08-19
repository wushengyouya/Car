using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using BLLFactory;
using Common;
using Model;
using WebUI.Models;

namespace WebUI.Controllers
{
    [RoutePrefix("usermanager")]
    [Admin]
    public class UserManagerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserManagerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //用户评论
        [Route("comment")]
        public ActionResult UserComment()
        {
            List<CommentModel> list = new List<CommentModel>();
            List<Comment> comments = _unitOfWork.CommentService.GetComments();

            foreach (var item in comments)
            {
                list.Add(new CommentModel
                {
                    Goods = item.Goods,
                    UserInfo = item.UserInfo,
                    Comment = item
                });
            }
            return View(list);
        }

        //搜索评论
        [Route("searchcomment")]
        public ActionResult SearchComment(string keyword)
        {
            Session["text"] = "搜索评论";
            List<CommentModel> commentModels = new List<CommentModel>();
            List<Comment> comments = _unitOfWork.CommentService.GetComments(keyword);
            foreach (var item in comments)
            {
                commentModels.Add(new CommentModel
                {
                    Goods = item.Goods,
                    UserInfo = item.UserInfo,
                    Comment = item
                });
            }
            return View(commentModels);
        }

        [Route("searchuser")]
        public ActionResult SearchUser(string keyword)
        {
            Session["text"] = "搜索用户";
            List<UserInfo> list = _unitOfWork.UserInfoService.GetUsers(keyword);
            List<UsersModel> usersModels = new List<UsersModel>();
            foreach (var item in list)
            {
                usersModels.Add(new UsersModel
                {
                    UserFlag = Tool.UserFlagToString((Tool.UserFlag) item.user_flag),
                    UserId = item.user_id,
                    UserName = item.user_name,
                    UserTel = item.user_tel,
                    UserHeadPortrait = item.user_head_portrait,
                    UserEmail = item.user_email,
                    RegistTime = item.regist_time,
                    UserSex = item.user_sex,
                    UserAdress = item.user_adress
                });
            }
            return View(usersModels);
        }

        //购物车
        [Route("shopCart")]
        public ActionResult UserShopCart()
        {
            return View();
        }

        /// <summary>
        /// 所有用户
        /// </summary>
        /// <returns></returns>
        [Route]
        public ActionResult Users()
        {
            var usersList = _unitOfWork.UserInfoService.GetInfosList();

            var usersModelList = new List<UsersModel>();


            foreach (var item in usersList)
            {
                UsersModel usersModel = new UsersModel
                {
                    UserAdress = item.user_adress,
                    UserEmail = item.user_email,
                    UserFlag = Tool.UserFlagToString((Tool.UserFlag)item.user_flag),
                    UserHeadPortrait = item.user_head_portrait,
                    UserId = item.user_id,
                    UserName = item.user_name,
                    UserSex = item.user_sex,
                    UserTel = item.user_tel,
                    RegistTime = item.regist_time
                };
                usersModelList.Add(usersModel);
            }
            return View(usersModelList);
        }

        [Route("detail/{id}")]
        public ActionResult UserDetail(int id)
        {
            Session["text"] = "用户详情";
            UserDetailModel userDetailModel = new UserDetailModel()
            {
                UserInfo = _unitOfWork.UserInfoService.GetUserInfoById(id),
                Comments = _unitOfWork.CommentService.GetCommentsByUserId(id).OrderByDescending(m=>m.comment_time).ToList(),
                ShopCartGoodses = _unitOfWork.ShopCartGoodsService.GetShopCartGoods(id).OrderByDescending(m=>m.goods_AddTime).ToList(),
                OrderInfos = _unitOfWork.OrderInfoService.GetOrdersListByUserId(id)
               
            };
            return View(userDetailModel);
        }

        
    }
}