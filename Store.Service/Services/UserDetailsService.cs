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
    public class UserDetailsService : IUserDetailsService
    {
        private readonly IUserDetailRepository UserDetailsRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserDetailsService(IUserDetailRepository UserDetailsRepository, IUnitOfWork unitOfWork)
        {
            this.UserDetailsRepository = UserDetailsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IUserDetailsService Members

        public UserDetails GetUserDetails(int id)
        {
            var UserDetails = UserDetailsRepository.GetById(id);
            return UserDetails;
        }

        public UserDetails GetUserDetails(string UserId)
        {
            var UserDetails = UserDetailsRepository.GetByUserId(UserId);
            return UserDetails;
        }

        public void CreateUserDetails(UserDetails UserDetails)
        {
            UserDetailsRepository.Add(UserDetails);
        }

        public void RemoveUserDetails(int Id)
        {
            var UserDetails = UserDetailsRepository.GetById(Id);

            UserDetailsRepository.Delete(UserDetails);
        }

        public void SaveUserDetails()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
