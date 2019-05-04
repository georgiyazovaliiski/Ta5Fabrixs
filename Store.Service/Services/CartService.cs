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
    public class CartService : ICartService
    {
        private readonly ICartRepository CartsRepository;
        private readonly ICookieCartRepository CookieCartsRepository;
        private readonly IUnitOfWork unitOfWork;

        public CartService(ICartRepository CartsRepository, ICookieCartRepository CookieCartsRepository, IUnitOfWork unitOfWork)
        {
            this.CartsRepository = CartsRepository;
            this.CookieCartsRepository = CookieCartsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICartService Members

        public Cart GetCart(string id)
        {
            var Cart = CartsRepository.GetCart(id);
            if(Cart!=null)
            return Cart;
            else
            {
                Cart ReturningCart = new Cart();
                var CookieCart = CookieCartsRepository.GetCookieCart(id);
                ReturningCart.ProductsInCart = CookieCart.ProductsInCart;
                ReturningCart.LastUpdated = CookieCart.LastUpdated;
                ReturningCart.WholePrice = CookieCart.WholePrice;
                ReturningCart.UserId = id;
                return ReturningCart;                
            }
        }

        public void CreateCart(Cart Cart)
        {
            CartsRepository.Add(Cart);
        }

        public void RemoveCart(int Id)
        {
            var Cart = CartsRepository.GetById(Id);

            CartsRepository.Delete(Cart);
        }

        public void SaveCart()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
