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
    public class PromoCodeRepository : RepositoryBase<PromoCode>, IPromoCodeRepository
    {
        public PromoCodeRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public PromoCode GetByName(string Name)
        {
            return this.DbContext.PromoCodes.Where(a => a.Code.Equals(Name)).FirstOrDefault();
        }
    }
}
