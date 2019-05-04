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
    public class CookieCartRepository : RepositoryBase<CookieCart>, ICookieCartRepository
    {
        public CookieCartRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public CookieCart GetCookieCart(string CookieCartId)
        {
            return this.DbContext.CookieCarts
                .Include(a => a.ProductsInCart)
                .Include(a => a.ProductsInCart.Select(b => b.Product))
                .Include(a => a.ProductsInCart.Select(b => b.Product.Images))
                .Include(a => a.ProductsInCart.Select(b => b.Order))
                .Where(a => a.CookieId.Equals(CookieCartId))
                .FirstOrDefault();
        }
        public override void Update(CookieCart entity)
        {
            entity.LastUpdated = DateTime.Now;
            base.Update(entity);
        }
        public override void Add(CookieCart entity)
        {
            entity.LastUpdated = DateTime.Now;
            base.Add(entity);
        }
    }
}
