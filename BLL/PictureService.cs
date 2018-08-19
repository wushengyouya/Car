using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using IDAL;
using Model;

namespace BLL
{
    public class PictureService : BaseService<Picture>, IPictureService
    {
        public override void SetDal()
        {
            CurrentDal = DbSession.PictureDal;
        }

        public List<Picture> GetGoodsPictures(int id)
        {
            return LoadEntities(m => m.goods_id == id).ToList();
        }

        public Picture GetPicture(int picId)
        {
            return LoadEntities(m => m.picture_id==picId).FirstOrDefault();
        }
    }
}
