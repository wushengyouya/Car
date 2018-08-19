using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface IPictureService:IBaseService<Picture>
    {
        /// <summary>
        /// 查找商品图片
        /// </summary>
        /// <param name="id">商品编号</param>
        /// <returns></returns>
        List<Picture> GetGoodsPictures(int id);

        /// <summary>
        /// 查询图片
        /// </summary>
        /// <param name="picId">图片id</param>
        /// <returns></returns>
        Picture GetPicture(int picId);
    }
}
