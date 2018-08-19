using IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace DALFactory
{
    public class DbSession : IDbSession
    {
        public DbContext Db => DbContextFactory.GetDbContext();

        private IUserInfoDal _userInfoDal;//优化创建
        /// <summary>
        /// 用户的数据操作实例
        /// </summary>
        public IUserInfoDal UserInfoDal
        {
            get
            {
                if (_userInfoDal==null)
                {
                    _userInfoDal = AbstractFactory.GetUserInfoDal();//通过抽象工厂封装了数据层。类实例的创建
                }

                return _userInfoDal;
            } 
            set { _userInfoDal = value; }
        }

        private IGoodsTypeDal _goodsTypeDal;
        public IGoodsTypeDal GoodsTypeDal
        {
            get
            {
                if (_goodsTypeDal==null)
                {
                    _goodsTypeDal = AbstractFactory.GetGoodsTypeDal();
                }

                return _goodsTypeDal;
            }
            set { _goodsTypeDal = value; }
        }

        private IGoodsDal _goodsDal;
        public IGoodsDal GoodsDal
        {
            get
            {
                if (_goodsDal==null)
                {
                    _goodsDal = AbstractFactory.GetGoodsDal();
                }

                return _goodsDal;
            }
            set { _goodsDal = value; }
        }


        private IPictureDal _pictureDal;
        public IPictureDal PictureDal
        {
            get
            {
                if (_pictureDal==null)
                {
                    _pictureDal = AbstractFactory.GetPictureDal();
                }

                return _pictureDal;
            }
            set { _pictureDal = value; }
        }


        //购物车
        private IShopCartGoodsDal _shopCartGoodsDal;
        public IShopCartGoodsDal ShopCartGoodsDal
        {
            get
            {
                if (_shopCartGoodsDal==null)
                {
                    _shopCartGoodsDal = AbstractFactory.GetShopCartGoodsDal();
                }

                return _shopCartGoodsDal;
            }
            set { _shopCartGoodsDal = value; }
        }

        //收藏
        private ICollectGoodsDal _collectGoodsDal;
        public ICollectGoodsDal CollectGoodsDal
        {
            get
            {
                if (_collectGoodsDal==null)
                {
                    _collectGoodsDal = AbstractFactory.GetCollectGoodsDal();
                }

                return _collectGoodsDal;
            }
            set { _collectGoodsDal = value; }
        }

        //订单
        private IOrderInfoDal _orderInfoDal;
        public IOrderInfoDal OrderInfoDal
        {
            get
            {
                if (_orderInfoDal==null)
                {
                    _orderInfoDal = AbstractFactory.GetOrderInfoDal();
                }

                return _orderInfoDal;
            }
            set { _orderInfoDal = value; }
        }

        //订单商品
        private IOrderGoodsDal _orderGoodsDal;
        public IOrderGoodsDal OrderGoodsDal
        {
            get
            {
                if (_orderGoodsDal==null)
                {
                    _orderGoodsDal = AbstractFactory.GetOrderGoodsDal();
                }

                return _orderGoodsDal;
            }
            set { _orderGoodsDal = value; }
        }


        private ICommentDal _commentDal;

        public ICommentDal CommentDal
        {
            get
            {
                if (_commentDal==null)
                {
                    _commentDal = AbstractFactory.GetCommentDal();
                }

                return _commentDal;
            }
            set { _commentDal = value; }
        }

        private IAdminDal _adminDal;

        public IAdminDal AdminDal
        {
            get => _adminDal ?? (_adminDal = AbstractFactory.GetAdminDal());
            set => _adminDal = value;
        }

        /// <summary>
        /// 保存所有操作至数据库--工作单元模式
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            try
            {
                return Db.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
