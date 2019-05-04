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
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    

        public ApplicationUser GetById(string id)
        {
            return this.DbContext.Users
                .Include(a => a.Cart.ProductsInCart)
                .Include(a=>a.Orders)
                .Where(a=>a.Id.Equals(id))
                .FirstOrDefault();
        }

        public void AddEmail(string mailName)
        {
            Email mail = new Email() { EmailName = mailName };
            this.DbContext.Emails.Add(mail);
        }

        public void RemoveEmail(string mailName)
        {
            var mail = this.DbContext.Emails.Where(a => a.EmailName.Equals(mailName)).FirstOrDefault();
            this.DbContext.Emails.Remove(mail);
        }
    }
}
