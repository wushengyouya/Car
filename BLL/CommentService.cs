using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using Model;

namespace BLL
{
    public class CommentService : BaseService<Comment>, ICommentService
    {
        public override void SetDal()
        {
            CurrentDal = DbSession.CommentDal;
        }

        public List<Comment> FindCommentsByGoodsId(int goodsId)
        {
            return LoadEntities(m => m.goods_id == goodsId).ToList();
        }

        public List<Comment> GetComments()
        {
            return LoadEntities(m => true).ToList();
        }

        public List<Comment> GetComments(string keyword)
        {
            IEnumerable<Comment> comments = LoadEntities(m => m.comment_content.Equals(keyword)).ToList();
            if (!comments.Any())
            {
                comments = LoadEntities(m => m.UserInfo.user_name.Equals(keyword)).ToList();
                if (!comments.Any())
                {
                    comments = LoadEntities(m => m.Goods.goods_name.Equals(keyword));
                    if (!comments.Any())
                    {
                        //将模糊查询拼接
                        var tempComments = LoadEntities(m => m.comment_content.Contains(keyword))//评论词模糊查询
                            .Concat(LoadEntities(m => m.UserInfo.user_name.Contains(keyword))) //用户名模糊查询
                            .Concat(LoadEntities(m => m.Goods.goods_name.Contains(keyword))); //商品名模糊查询

                        comments = tempComments;

                    }

                }
            }

            return comments.ToList();
        }

        public List<Comment> GetComments(int commentId)
        {
            return LoadEntities(m => m.comment_id == commentId).ToList();
        }

        public List<Comment> GetCommentsByUserId(int userId)
        {
            return LoadEntities(m => m.user_id == userId).ToList();
        }

        /// <summary>
        /// 获取最新评论
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public List<Comment> GetNewestComments(int goodsId)
        {

            return LoadEntities(m => m.goods_id == goodsId).Where(m => DateTime.Now.Day - m.comment_time.Day <= 2).ToList();
        }

        /// <summary>
        /// 获取时间倒序的评论
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public List<Comment> GetCommentsOrderByTime(int goodsId)
        {

            return LoadEntities(m => m.goods_id == goodsId).OrderByDescending(m => m.comment_time).ToList();
        }
    }
}
