using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebUI.Models
{
    public class CommentModel
    {
        public UserInfo UserInfo { get; set; }
        public Comment Comment { get; set; }
        public Goods Goods { get; set; }
    }
}