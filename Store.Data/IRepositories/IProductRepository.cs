using Store.Data.Infrastructure;
using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> GetProductByCategory(string categoryName);
        List<Product> GetProductsByName(string Name);
        List<Product> GetNewProducts();
        List<SizeAndProductId> GetProductSizes(string Name);
        List<Product> GetRelatedProducts(int Id);
    }
}
