﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarEntities : DbContext
    {
        public CarEntities()
            : base("name=CarEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<CollectGoods> CollectGoods { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Goods> Goods { get; set; }
        public virtual DbSet<GoodsType> GoodsType { get; set; }
        public virtual DbSet<Order_Goods> Order_Goods { get; set; }
        public virtual DbSet<OrderInfo> OrderInfo { get; set; }
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<Reply> Reply { get; set; }
        public virtual DbSet<ShopCart_Goods> ShopCart_Goods { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
    }
}
