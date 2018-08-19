using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebUI.Models
{
    public class GoodsTypeModel
    {
        public string GoodsTypeName { get; set; }
        public List<GoodsModel> Goodses { get; set; }
    }
}