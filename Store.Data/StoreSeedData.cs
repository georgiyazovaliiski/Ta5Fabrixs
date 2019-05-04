using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Store.Model;
using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Store.Data
{
    public class StoreSeedData : DropCreateDatabaseAlways<StoreEntities>
    {
        protected override void Seed(StoreEntities context)
        {
            GetLayouts().ForEach(g => context.LayoutModels.Add(g));
            GetPromoCodes().ForEach(c => context.PromoCodes.Add(c));
            GetProductCategories().ForEach(g => context.ProductCategory.Add(g));
            context.Commit();
            GetProducts().ForEach(g => context.Products.Add(g));

            context.Commit();


            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            
            string name = "admin@abv.bg";
            string password = "123456";
            string test = "test";
            
            var admin = new ApplicationUser() { UserName = name, Email = name, FirstName = "Admin", LastName = "Dean", Cart = new Cart() { LastUpdated = DateTime.Now }, Orders = new List<Order>() };
            var guest = new ApplicationUser() { Id = "guestid", UserName = "test", Email = "test", FirstName = "Guest", LastName = "User", Cart = new Cart() { LastUpdated = DateTime.Now }, Orders = new List<Order>() };

            //Create Role Test and User Test
            RoleManager.Create(new IdentityRole(test));
            var AdminResult = UserManager.Create(admin, password);
            UserManager.Create(guest, password);

            //Create Role Admin if it does not exist
            if (!RoleManager.RoleExists(name))
            {
                var roleresult = RoleManager.Create(new IdentityRole(name.Split('@')[0]));
            }

            //Create User=Admin with password=123456
            /*
            var user = new ApplicationUser();

            user.UserName = name;
            var adminresult = UserManager.Create(admin, password);*/

            //Add User Admin to Role Admin
            if (AdminResult.Succeeded)
            {
                var result = UserManager.AddToRole(admin.Id, name.Split('@')[0]);
            }
            base.Seed(context);

        }

        private static List<PromoCode> GetPromoCodes()
        {
            return new List<PromoCode>
            {
                new PromoCode {
                    Code = "Ta5",
                    PercentageDiscount = 5
                },
                new PromoCode {
                    Code = "RareCode",
                    PercentageDiscount = 15
                },
                new PromoCode {
                    Code = "FullDiscount",
                    PercentageDiscount = 100
                }
            };
        }

        private static List<LayoutModel> GetLayouts()
        {
            return new List<LayoutModel>
            {
                new LayoutModel {
                    Banner1Url = "banner.jpg",
                    Banner2Url = "banner.jpg",
                    Banner3Url = "banner.jpg",
                    NewReleasesBannerUrl = "banner.jpg",
                    FrontCategoryBanner1Id = 1,
                    FrontCategoryBanner2Id = 2,
                    FrontCategoryBanner3Id = 3
                }
            };
        }

        private static List<ProductCategory> GetProductCategories()
        {
            return new List<ProductCategory>
            {
                new ProductCategory {
                    Name = "TShirts",
                    UrlImage = "banner.jpg"
                },
                new ProductCategory {
                    Name = "Hoodies",
                    UrlImage = "banner.jpg"
                },
                new ProductCategory {
                    Name = "Jeans",
                    UrlImage = "banner.jpg"
                }
            };
        }
    
        private static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { 
                    Name = "TShirt",
                    Description = "TShirt for sale",
                    Quantity = 3,
                    PriceEU = 345,
                    Size = Size.M,
                    Tags = new List<ItemTag>{ new ItemTag { TagName = "Tag1" }, new ItemTag { TagName = "Tag2" } },
                    TagsText = "tag1,tag2",
                    CategoryId = 2,
                    ReleaseDate = DateTime.Now
                } 
            };
        }
    }
}
