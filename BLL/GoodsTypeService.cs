using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using Model;

namespace BLL
{
    public class GoodsTypeService:BaseService<GoodsType>,IGoodsTypeService
    {
        public override void SetDal()
        {
            CurrentDal = DbSession.GoodsTypeDal;
        }

        public GoodsType GetGoodsType(int? id)
        {
            return LoadEntities(m => m.goods_type_id == id).FirstOrDefault();
        }

        public GoodsType GetGoodsType(string typeName)
        {
            return LoadEntities(m => m.goods_type_name.Equals(typeName)).FirstOrDefault();
        }

        public List<GoodsType> GetGoodsTypeList()
        {
            return LoadEntities(m => true).ToList();
        }
    }
}
