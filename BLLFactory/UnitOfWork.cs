using System.Data.Entity;
using BLL;
using DAL;
using IBLL;

namespace BLLFactory
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(
            ICollectGoodsService collectGoodsService,
            ICommentService commentService,
            IGoodsService goodsService,
            IGoodsTypeService goodsTypeService,
            IOrderGoodsService orderGoodsService,
            IOrderInfoService orderInfoService,
            IPictureService pictureService,
            IShopCartGoodsService shopCartGoodsService,
            IUserInfoService userInfoService,
            IAdminService adminService
            
            )
        {
            _dbContext = DbContextFactory.GetDbContext();
            CollectGoodsService = collectGoodsService;
            CommentService = commentService;
            GoodsService = goodsService;
            GoodsTypeService = goodsTypeService;
            OrderGoodsService = orderGoodsService;
            OrderInfoService = orderInfoService;
            PictureService = pictureService;
            ShopCartGoodsService = shopCartGoodsService;
            UserInfoService = userInfoService;
            AdminService = adminService;

        }
        public ICollectGoodsService CollectGoodsService { get; }
        public ICommentService CommentService { get; }
        public IGoodsService GoodsService { get; }
        public IGoodsTypeService GoodsTypeService { get; }
        public IOrderGoodsService OrderGoodsService { get; }
        public IOrderInfoService OrderInfoService { get; }
        public IPictureService PictureService { get; }
        public IShopCartGoodsService ShopCartGoodsService { get; }
        public IUserInfoService UserInfoService { get; }
        public IAdminService AdminService { get; }

        public int Result()
        {
            return _dbContext.SaveChanges();
        }
    }
}