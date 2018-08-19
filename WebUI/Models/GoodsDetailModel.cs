using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebUI.Models
{
    public class GoodsDetailModel
    {
        public List<Picture> PictureAdress { get; set; }
        public Goods Goods { get; set; }
        public GoodsType GoodsType { get; set; }
        public List<CommentModel> Comments { get; set; }
        public List<Comment> CommentsOrderByTime { get; set; }
        public bool UserIsCollectGoods { get; set; }

    }
}