using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface ICommentService:IBaseService<Comment>
    {
        /// <summary>
        /// 查找评论
        /// </summary>
        /// <param name="goodsId">商品编号</param>
        /// <returns></returns>
        List<Comment> FindCommentsByGoodsId(int goodsId);
        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <returns></returns>
        List<Comment> GetComments();
        /// <summary>
        /// 通过评论类容查找评论
        /// </summary>
        /// <param name="content">评论的关键词</param>
        /// <returns></returns>
        List<Comment> GetComments(string content);

        /// <summary>
        /// 通过评论编号查找评论
        /// </summary>
        /// <param name="commentId">评论编号</param>
        /// <returns></returns>
        List<Comment> GetComments(int commentId);
        /// <summary>
        /// 通过用户编号查询评论
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Comment> GetCommentsByUserId(int userId);
        List<Comment> GetNewestComments(int goodsId);
        /// <summary>
        /// 查找某个商品的评论并通过时间排序
        /// </summary>
        /// <param name="goodsId">商品编号</param>
        /// <returns></returns>
        List<Comment> GetCommentsOrderByTime(int goodsId);
    }
}
