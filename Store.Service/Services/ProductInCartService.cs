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
    public class ProductInCartService : IProductInCartService
    {
        private readonly IProductInCartRepository ProductInCartsRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductInCartService(IProductInCartRepository ProductInCartsRepository, IUnitOfWork unitOfWork)
        {
            this.ProductInCartsRepository = ProductInCartsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IProductInCartService Members

        public ProductInCart GetProductInCart(int id)
        {
            var ProductInCart = ProductInCartsRepository.GetProductInCart(id);
            return ProductInCart;
        }

        public void CreateProductInCart(ProductInCart ProductInCart)
        {
            ProductInCartsRepository.Add(ProductInCart);
        }

        public void RemoveProductInCart(int Id)
        {
            var ProductInCart = ProductInCartsRepository.GetById(Id);

            ProductInCartsRepository.Delete(ProductInCart);
        }

        public void UpdateProductInCart(ProductInCart ProductInCart)
        {
            ProductInCartsRepository.Update(ProductInCart);
        }

        public void SaveProductInCart()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
