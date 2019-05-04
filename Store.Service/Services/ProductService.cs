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

namespace Store.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository ProductsRepository;
        private readonly IUnitOfWork unitOfWork;


        public ProductService(IProductRepository ProductsRepository, IUnitOfWork unitOfWork)
        {
            this.ProductsRepository = ProductsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IProductService Members

        public Product GetProduct(int id)
        {
            var Product = ProductsRepository.GetById(id);
            return Product;
        }

        public List<Product> GetProducts(string name)
        {
            var Products = ProductsRepository.GetProductByCategory(name);
            return Products;
        }

        public List<Product> GetProductsByName(string name)
        {
            var Products = ProductsRepository.GetProductsByName(name);
            return Products;
        }

        public List<Product> GetRelatedItems(int Id)
        {
            var Products = ProductsRepository.GetRelatedProducts(Id);
            return Products;
        }

        public void CreateProduct(Product Product)
        {
            ProductsRepository.Add(Product);
        }

        public void UpdateProduct(Product Product)
        {
            ProductsRepository.Update(Product);
        }

        public void RemoveProduct(int Id)
        {
            var product = ProductsRepository.GetById(Id);
            ProductsRepository.Delete(product);
        }

        public List<Product> GetNewProducts()
        {
            return ProductsRepository.GetNewProducts();
        }

        public List<SizeAndProductId> GetProductSizes(string Name)
        {
            return ProductsRepository.GetProductSizes(Name);
        }

        public void SaveProduct()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
