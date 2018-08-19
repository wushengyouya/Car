using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using Model;

namespace BLL
{
    public class UserInfoService:BaseService<UserInfo>,IUserInfoService
    {
        public override void SetDal()
        {
            CurrentDal = DbSession.UserInfoDal;
        }

        /// <summary>
        /// 校检用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public UserInfo CheckUser(string name, string pwd)
        {
            UserInfo user = LoadEntities(m=>m.user_name==name).FirstOrDefault();

            if (user!=null)
            {
                if (user.user_pwd.Equals(pwd)||Tool.GetMd5(pwd).Equals(user.user_pwd))
                {
                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// 通过用户id查找用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoById(int? userId)
        {
            return LoadEntities(m => m.user_id == userId).FirstOrDefault();
        }

        public UserInfo GetUserInfoByName(string userName)
        {
            return LoadEntities(m => m.user_name.Equals(userName)).FirstOrDefault();
        }

        /// <summary>
        /// 查找所有用户
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetInfosList()
        {
            return LoadEntities(m => true).ToList();
        }

        public List<UserInfo> GetUsers(string userName)
        {
            IEnumerable<UserInfo> userInfos = LoadEntities(m => m.user_name.Contains(userName)).ToList();
            if (!userInfos.Any())
            {
                userInfos = LoadEntities(m => m.user_name.Contains(userName));
            }

            return userInfos.ToList();
        }
    }
}
