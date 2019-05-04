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
    public class UserDetailRepository : RepositoryBase<UserDetails>, IUserDetailRepository
    {
        public UserDetailRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public UserDetails GetByUserId(string UserId)
        {
            return this.DbContext.UserDetails.Where(a => a.UserId.Equals(UserId)).OrderByDescending(a=>a.Id).FirstOrDefault();
        }
    }
}
