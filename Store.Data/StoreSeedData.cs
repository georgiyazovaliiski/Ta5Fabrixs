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
            GetCategories().ForEach(c => context.Categories.Add(c));
            GetGadgets().ForEach(g => context.Gadgets.Add(g));
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

        private static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category {
                    Name = "Tablets"
                },
                new Category {
                    Name = "Laptops"
                },
                new Category {
                    Name = "Mobiles"
                }
            };
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

        private static List<Gadget> GetGadgets()
        {
            return new List<Gadget>
            {
                new Gadget {
                    Name = "ProntoTec 7",
                    Description = "Android 4.4 KitKat Tablet PC, Cortex A8 1.2 GHz Dual Core Processor,512MB / 4GB,Dual Camera,G-Sensor (Black)",
                    CategoryID = 1,
                    Price = 46.99m,
                    Image = "prontotec.jpg"
                },
                new Gadget {
                    Name = "Samsung Galaxy",
                    Description = "Android 4.4 Kit Kat OS, 1.2 GHz quad-core processor",
                    CategoryID = 1,
                    Price = 120.95m,
                    Image= "samsung-galaxy.jpg"
                },
                new Gadget {
                    Name = "NeuTab® N7 Pro 7",
                    Description = "NeuTab N7 Pro tablet features the amazing powerful, Quad Core processor performs approximately Double multitasking running speed, and is more reliable than ever",
                    CategoryID = 1,
                    Price = 59.99m,
                    Image= "neutab.jpg"
                },
                new Gadget {
                    Name = "Dragon Touch® Y88X 7",
                    Description = "Dragon Touch Y88X tablet featuring the incredible powerful Allwinner Quad Core A33, up to four times faster CPU, ensures faster multitasking speed than ever. With the super-portable size, you get a robust power in a device that can be taken everywhere",
                    CategoryID = 1,
                    Price = 54.99m,
                    Image= "dragon-touch.jpg"
                },
                new Gadget {
                    Name = "Alldaymall A88X 7",
                    Description = "This Alldaymall tablet featuring the incredible powerful Allwinner Quad Core A33, up to four times faster CPU, ensures faster multitasking speed than ever. With the super-portable size, you get a robust power in a device that can be taken everywhere",
                    CategoryID = 1,
                    Price = 47.99m,
                    Image= "Alldaymall.jpg"
                },
                new Gadget {
                    Name = "ASUS MeMO",
                    Description = "Pad 7 ME170CX-A1-BK 7-Inch 16GB Tablet. Dual-Core Intel Atom Z2520 1.2GHz CPU",
                    CategoryID = 1,
                    Price = 94.99m,
                    Image= "asus-memo.jpg"
                },
                new Gadget {
                    Name = "ASUS 15.6-Inch",
                    Description = "Latest Generation Intel Dual Core Celeron 2.16 GHz Processor (turbo to 2.41 GHz)",
                    CategoryID = 2,
                    Price = 249.5m,
                    Image = "asus-latest.jpg"
                },
                new Gadget {
                    Name = "HP Pavilion 15-r030wm",
                    Description = "This Certified Refurbished product is manufacturer refurbished, shows limited or no wear, and includes all original accessories plus a 90-day warranty",
                    CategoryID = 2,
                    Price = 299.95m,
                    Image = "hp-pavilion.jpg"
                },
                new Gadget {
                    Name = "Dell Inspiron 15.6-Inch",
                    Description = "Intel Celeron N2830 Processor, 15.6-Inch Screen, Intel HD Graphics",
                    CategoryID = 2,
                    Price = 308.00m,
                    Image = "dell-inspiron.jpg"
                },
                new Gadget {
                    Name = "Acer Aspire E Notebook",
                    Description = "15.6 HD Active Matrix TFT Color LED (1366 x 768) 16:9 CineCrystal Display",
                    CategoryID = 2,
                    Price = 299.95m,
                    Image = "acer-aspire.jpg"
                },
                new Gadget {
                    Name = "HP Stream 13",
                    Description = "Intel Celeron N2840 Processor. 2 GB DDR3L SDRAM, 32 GB Solid-State Drive and 1TB OneDrive Cloud Storage for one year",
                    CategoryID = 2,
                    Price = 202.99m,
                    Image = "hp-stream.jpg"
                },
                new Gadget {
                    Name = "Nokia Lumia 521",
                    Description = "T-Mobile Cell Phone 4G - White. 5MP Camera - Snap creative photos with built-in digital lenses",
                    CategoryID = 3,
                    Price = 63.99m,
                    Image = "nokia-lumia.jpg"
                },
                new Gadget {
                    Name = "HTC Desire 816",
                    Description = "13 MP Rear Facing BSI Camera / 5 MP Front Facing",
                    CategoryID = 3,
                    Price = 177.99m,
                    Image = "htc-desire.jpg"
                },
                new Gadget {
                    Name = "Sanyo Innuendo",
                    Description = "Uniquely designed 3G-enabled messaging phone with side-flipping QWERTY keyboard and external glow-thru OLED dial pad that 'disappears' when not in use",
                    CategoryID = 3,
                    Price = 54.99m,
                    Image = "sanyo-innuendo.jpg"
                },
                new Gadget {
                    Name = "Ulefone N9000",
                    Description = "Unlocked world GSM phone. 3G-850/2100, 2G -850/900/1800/1900",
                    CategoryID = 3,
                    Price = 133.99m,
                    Image = "ulefone.jpg"
                }
 
            };
        }
    }
}
