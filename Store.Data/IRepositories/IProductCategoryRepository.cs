using Store.Data.Infrastructure;
using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.IRepositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        ProductCategory GetByName(string Name);
        ProductCategory GetFirst();
        List<ProductCategory> GetCategoriesAndProducts();
        ProductCategory GetCategoryAndItsProductsByName(string Name);
        ProductCategory GetCategoryAndItsProductsById(int Id);
    }
}
