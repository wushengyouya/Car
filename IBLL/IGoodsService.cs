using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface IGoodsService:IBaseService<Goods>
    {
        /// <summary>
        /// 通过商品id查询商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Goods GetGoods(int id);
        /// <summary>
        /// 通过商品名字查询商品
        /// </summary>
        /// <param name="goodsName"></param>
        /// <returns></returns>
        List<Goods> GetGoods(string goodsName);
        /// <summary>
        /// 通过类别编号查询商品
        /// </summary>
        /// <param name="typeId">类别编号</param>
        /// <returns></returns>
        List<Goods> GetGoodsListByTypeId(int typeId);

        /// <summary>
        /// 通过类别名字查询商品
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        List<Goods> GetGoodsListByTypeName(string typeName);
        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <returns></returns>
        List<Goods> GetGoodsList();
        
        bool EditGoods(List<Goods> list);


    }
}
