using IBLL;

namespace BLLFactory
{
    public interface IUnitOfWork
    {
        ICollectGoodsService CollectGoodsService { get; }
        ICommentService CommentService { get; }
        IGoodsService GoodsService { get; }
        IGoodsTypeService GoodsTypeService { get; }
        IOrderGoodsService OrderGoodsService { get;}
        IOrderInfoService OrderInfoService { get; }
        IPictureService PictureService { get; }
        IShopCartGoodsService ShopCartGoodsService { get; }
        IUserInfoService UserInfoService { get; }
        IAdminService AdminService { get; }

        int Result();
    }
}