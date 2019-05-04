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
    public class CookieCartService : ICookieCartService
    {
        private readonly ICookieCartRepository CookieCartsRepository;
        private readonly IUnitOfWork unitOfWork;

        public CookieCartService(ICookieCartRepository CookieCartsRepository, IUnitOfWork unitOfWork)
        {
            this.CookieCartsRepository = CookieCartsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICookieCartService Members

        public CookieCart GetCookieCart(string id)
        {
            var CookieCart = CookieCartsRepository.GetCookieCart(id);
            return CookieCart;
        }

        public void UpdateCookieCart(CookieCart CookieCart)
        {
            CookieCartsRepository.Update(CookieCart);
        }

        public void CreateCookieCart(CookieCart CookieCart)
        {
            CookieCartsRepository.Add(CookieCart);
        }

        public void RemoveCookieCart(int Id)
        {
            var CookieCart = CookieCartsRepository.GetById(Id);

            CookieCartsRepository.Delete(CookieCart);
        }

        public void SaveCookieCart()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
