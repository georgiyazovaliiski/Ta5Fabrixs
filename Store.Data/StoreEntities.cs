using Microsoft.AspNet.Identity.EntityFramework;
using Store.Model;
using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data
{
    public class StoreEntities : IdentityDbContext<ApplicationUser>
    {
        public StoreEntities() 
            : base("StoreEntities", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;        		     Database.SetInitializer<StoreEntities>(null);
        }

        public static StoreEntities Create()
        {
            return new StoreEntities();
        }

        public DbSet<CookieCart> CookieCarts { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductInCart> ProductsInCarts { get; set; }
        public DbSet<Promo> Promo { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<ItemTag> ItemTags { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<LayoutModel> LayoutModels { get; set; }
        public DbSet<Email> Emails { get; set; }


        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
