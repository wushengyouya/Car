using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using Model;

namespace BLL
{
    public class GoodsService:BaseService<Goods>,IGoodsService
    {
        public override void SetDal()
        {
            CurrentDal = DbSession.GoodsDal;
        }

        public Goods GetGoods(int id)
        {
           
            return LoadEntities(m => m.goods_id == id).FirstOrDefault();
        }


        public List<Goods> GetGoods(string goodsName)
        {
            IEnumerable<Goods> goodses = LoadEntities(m => m.goods_name.Equals(goodsName));

            if (!goodses.Any())//无该名字的商品
            {
                goodses = LoadEntities(m => m.goods_title.Equals(goodsName));//通titlte再查
                if (!goodses.Any())//无该title的商品
                {
                    goodses = LoadEntities(m => m.goods_name.Contains(goodsName)).Concat(LoadEntities(m => m.goods_title.Contains(goodsName)));
                }

                
            }

            return goodses.ToList();


        }

        public List<Goods> GetGoodsListByTypeId(int typeId)
        {
            return LoadEntities(m => m.goods_type_id == typeId).ToList();
        }

        public List<Goods> GetGoodsListByTypeName(string typeName)
        {
            return LoadEntities(m => m.GoodsType.goods_type_name.Equals(typeName)).ToList();
        }

        public List<Goods> GetGoodsList()
        {
            return LoadEntities(m => true).ToList();
        }

        /// <summary>
        /// 修改多个商品
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool EditGoods(List<Goods> list)
        {
            bool b = false;
            foreach (Goods goods in list)
            {
                b = EditEntity(goods);
            }

            return b;
        }
      
    }
}
