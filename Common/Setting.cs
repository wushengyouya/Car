using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLLFactory;
using Model;

namespace Common
{
    public sealed class Setting
    {
        public static string GoodsPicFolder { get; set; } = "/GoodsPic/";
        public static string HeadPicFolder { get; set; } = "/HeadPic/";

        /// <summary>
        /// 一页显示多少条
        /// </summary>
        public static int PageSize { get; set; } = 8;
        public static List<GoodsType> GoodsTypes { get; set; }
        public static UserInfo UserInfo { get; set; } = null;
        public static IEnumerable<Goods> Goodses { get; set; }

       
    }
}
