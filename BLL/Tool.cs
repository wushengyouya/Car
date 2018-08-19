using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace BLL
{
 
    public sealed class Tool
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public enum OrderFlag { StayPay, StaySendOut, StayReceiving, StayComment, Received }

        /// <summary>
        /// 用户状态
        /// </summary>
        public enum UserFlag
        {
            /// <summary>
            /// 禁用
            /// </summary>
            Disable,
            /// <summary>
            /// 正常
            /// </summary>
            Normal
        }

        /// <summary>
        /// 商品状态
        /// </summary>
        public enum GoodsFlag
        {
            Disable,Normal
        }

        /// <summary>
        /// 评论状态
        /// </summary>
        public enum CommentFlag
        {
            Disable, Normal
        }

        /// <summary>
        /// 是否评论了商品
        /// </summary>
        public enum IsComment
        {
            No,Yes
        }

        /// <summary>
        /// 用户性别
        /// </summary>
        public enum UserGender
        {
            Man, Woman
        }

        /// <summary>
        /// 是否记住密码
        /// </summary>
        public enum IsRememberPwd
        {
            No, Yes
        }


        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public static string GetMd5(string userPwd)
        {
            byte[] buf = MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(userPwd));
            return BitConverter.ToString(buf).Replace("-", "");
        }

        /// <summary>
        /// 订单状态中文表示
        /// </summary>
        /// <param name="orderFlag"></param>
        /// <returns></returns>
        public static string OrderFlagToString(OrderFlag orderFlag)
        {
            string flag = null;
            switch (orderFlag)
            {
                case OrderFlag.StayPay:
                    flag = "待支付";
                    break;
                case OrderFlag.StaySendOut:
                    flag = "待发货";
                    break;
                case OrderFlag.StayReceiving:
                    flag = "待收货";
                    break;
                case OrderFlag.StayComment:
                    flag = "待评论";
                    break;
                case OrderFlag.Received:
                    flag = "已收货";
                    break;

            }

            return flag;
        }

        /// <summary>
        /// 用户状态
        /// </summary>
        /// <param name="userFlag"></param>
        /// <returns></returns>
        public static string UserFlagToString(UserFlag userFlag)
        {
            switch (userFlag)
            {
                case UserFlag.Disable:
                    return "禁用";
                case UserFlag.Normal:
                    return "正常";
            }

            return null;
        }

       
        /// <summary>
        /// 用户评论
        /// </summary>
        /// <param name="commentFlag"></param>
        /// <returns></returns>
        public static string CommentFlagTostring(CommentFlag commentFlag)
        {
            switch (commentFlag)
            {
                case CommentFlag.Disable:
                    return "禁用";
                case CommentFlag.Normal:
                    return "正常";
            }
            return null;
        }
    }
}
