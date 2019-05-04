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
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public ProductCategory GetByName(string Name)
        {
            return this.DbContext.ProductCategory.Where(a => a.Name == Name).FirstOrDefault();
        }

        public List<ProductCategory> GetCategoriesAndProducts()
        {
            return this.DbContext.ProductCategory.Include(a => a.Products).ToList();
        }

        public ProductCategory GetCategoryAndItsProductsById(int Id)
        {
            return this.DbContext.ProductCategory.Include(a => a.Products.Select(b => b.Images)).Where(a => a.Id == Id).FirstOrDefault();
        }

        public ProductCategory GetCategoryAndItsProductsByName(string Name)
        {
            var result = this.DbContext.ProductCategory.Include(a => a.Products.Select(v => v.Images)).Where(a => a.Name.Equals(Name)).FirstOrDefault();

            return result;
        }

        public ProductCategory GetFirst()
        {
            return this.DbContext.ProductCategory.First();
        }
    }
}
