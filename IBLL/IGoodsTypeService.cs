using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface IGoodsTypeService :IBaseService<GoodsType>
    {
        /// <summary>
        /// 根据类别获取商品类别
        /// </summary>
        /// <param name="id">类别编号</param>
        /// <returns></returns>
        GoodsType GetGoodsType(int? id);

        /// <summary>
        /// 根据类别查询
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        GoodsType GetGoodsType(string typeName);
        /// <summary>
        /// 获取所有类别
        /// </summary>
        /// <returns></returns>
        List<GoodsType> GetGoodsTypeList();
    }
}
