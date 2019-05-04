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
    public class LayoutRepository : RepositoryBase<LayoutModel>, ILayoutRepository
    {
        public LayoutRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public override LayoutModel GetById(int id)
        {
            return this.DbContext.LayoutModels
                .Where(a=>a.Id == id)
                .FirstOrDefault();
        }
    }
}
