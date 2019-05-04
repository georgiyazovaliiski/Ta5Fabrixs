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
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Cart GetCart(string CartId)
        {
            return this.DbContext.Carts
                .Include(a => a.ProductsInCart)
                .Include(a => a.ProductsInCart.Select(b => b.Product))
                .Include(a => a.ProductsInCart.Select(b => b.Product.Images))
                .Include(a => a.ProductsInCart.Select(b => b.Order))
                .Where(a => a.UserId.Equals(CartId))
                .FirstOrDefault();
        }
        public override void Update(Cart entity)
        {
            entity.LastUpdated = DateTime.Now;
            base.Update(entity);
        }
    }
}
