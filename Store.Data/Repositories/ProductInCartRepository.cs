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
    public class ProductInCartRepository : RepositoryBase<ProductInCart>, IProductInCartRepository
    {
        public ProductInCartRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public ProductInCart GetProductInCart(int ProductInCartId)
        {
            return this.DbContext.ProductsInCarts
                .Include(a => a.Product)
                .Where(a=>a.Id == ProductInCartId)
                .FirstOrDefault();
        }
    }
}
