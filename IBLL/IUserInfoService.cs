using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface IUserInfoService: IBaseService<UserInfo>
    {
        //写特有业务
        UserInfo CheckUser(string name, string pwd);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        UserInfo GetUserInfoById(int? userId);
        UserInfo GetUserInfoByName(string userName);
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        List<UserInfo> GetInfosList();
        /// <summary>
        /// 通过用户名模糊查找用户
        /// </summary>
        /// <param name="userName">用户名关键词</param>
        /// <returns></returns>
        List<UserInfo> GetUsers(string userName);
    }
}
