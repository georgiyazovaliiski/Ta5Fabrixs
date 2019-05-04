using Store.Data.Infrastructure;
using Store.Data.IRepositories;
using Store.Data.Repositories;
using Store.Model;
using Store.Model.Models;
using Store.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
namespace Store.Service
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository ApplicationUsersRepository;
        private readonly IUnitOfWork unitOfWork;

        public ApplicationUserService(IApplicationUserRepository ApplicationUsersRepository, IUnitOfWork unitOfWork)
        {
            this.ApplicationUsersRepository = ApplicationUsersRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IApplicationUserService Members

        public ApplicationUser GetApplicationUser(string id)
        {
            var ApplicationUser = ApplicationUsersRepository.GetById(id);
            ApplicationUser.Cart.ProductsInCart = ApplicationUser.Cart.ProductsInCart.Where(a => a.Order == null).ToList();
            return ApplicationUser;
        }

        public void CreateApplicationUser(ApplicationUser ApplicationUser)
        {
            ApplicationUsersRepository.Add(ApplicationUser);
        }

        public void RemoveApplicationUser(string Id)
        {
            var ApplicationUser = ApplicationUsersRepository.GetById(Id);

            ApplicationUsersRepository.Delete(ApplicationUser);
        }

        public void UpdateApplicationUser(ApplicationUser ApplicationUser)
        {
            ApplicationUsersRepository.Update(ApplicationUser);
        }

        public void SaveApplicationUser()
        {
            unitOfWork.Commit();
        }

        public ApplicationUser GetApplicationUser(int id)
        {
            throw new NotImplementedException();
        }

        public void AddMail(string mailName)
        {
            this.ApplicationUsersRepository.AddEmail(mailName);
        }

        #endregion
    }
}
