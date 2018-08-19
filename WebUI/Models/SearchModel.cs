using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class SearchModel
    {
        public string GoodsName { get; set; }
        public List<GoodsModel> GoodsModels { get; set; }
    }
}