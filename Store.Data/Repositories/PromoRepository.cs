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
    public class PromoRepository : RepositoryBase<Promo>, IPromoRepository
    {
        public PromoRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public override IEnumerable<Promo> GetAll()
        {
            return this.DbContext.Promo.Include(a => a.Product);
        }

        public override Promo GetById(int id)
        {
            return this.DbContext.Promo.Include(a => a.Product).Where(a=>a.ProductId == id).FirstOrDefault();
        }

        public List<Promo> GetEndingPromos()
        {
            var now = DateTime.Now;
            return this.DbContext.Promo.Include(a=>a.Product).Include(a=>a.Product.Images).Where(a => a.EndDate > now).OrderBy(a => a.EndDate).ToList();
        }
    }
}
