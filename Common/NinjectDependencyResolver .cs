using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using BLLFactory;
using IBLL;
using Ninject;

namespace Common
{
    public class NinjectDependencyResolver: IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public void AddBindings()
        {
            _kernel.Bind<ICollectGoodsService>().To<CollectGoodsService>();
            _kernel.Bind<IGoodsService>().To<GoodsService>();
            _kernel.Bind<IGoodsTypeService>().To<GoodsTypeService>();
            _kernel.Bind<IPictureService>().To<PictureService>();
            _kernel.Bind<IUserInfoService>().To<UserInfoService>();
            _kernel.Bind<IShopCartGoodsService>().To<ShopCartGoodsService>();
            _kernel.Bind<ICommentService>().To<CommentService>();
           
            _kernel.Bind<IOrderInfoService>().To<OrderInfoService>();
            _kernel.Bind<IOrderGoodsService>().To<OrderGoodsService>();

            _kernel.Bind<IAdminService>().To<AdminService>();

            _kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
           
        }
    }
}