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
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public override Order GetById(int id)
        {
            return this.DbContext.Orders
                .Include(a => a.ProductsInCart.Select(b=>b.Product))
                .Where(a=>a.Id == id)
                .FirstOrDefault();
        }
    }
}
