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
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository ProductCategorysRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductCategoryService(IProductCategoryRepository ProductCategorysRepository, IUnitOfWork unitOfWork)
        {
            this.ProductCategorysRepository = ProductCategorysRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IProductCategoryService Members

        public ProductCategory GetProductCategory(int id)
        {
            var ProductCategory = ProductCategorysRepository.GetById(id);
            return ProductCategory;
        }

        public List<ProductCategory> GetProductCategories()
        {
            var ProductCategories = ProductCategorysRepository.GetAll().ToList();
            return ProductCategories;
        }

        public void CreateProductCategory(ProductCategory ProductCategory)
        {
            ProductCategorysRepository.Add(ProductCategory);
        }

        public void RemoveProductCategory(int Id)
        {
            var ProductCategory = ProductCategorysRepository.GetById(Id);
            ProductCategorysRepository.Delete(ProductCategory);
        }

        public ProductCategory GetProductCategory(string name)
        {
            if (name.Equals(""))
            {
                return ProductCategorysRepository.GetFirst();
            }
            return ProductCategorysRepository.GetByName(name);
        }

        public void UpdateProductCategory(ProductCategory ProductCategory)
        {
            ProductCategorysRepository.Update(ProductCategory);
        }

        public List<ProductCategory> GetCategoriesAndProducts()
        {
            return ProductCategorysRepository.GetCategoriesAndProducts();
        }

        public ProductCategory GetCategoryAndItsProducts(string name)
        {
            return ProductCategorysRepository.GetCategoryAndItsProductsByName(name);
        }

        public ProductCategory GetCategoryAndItsProducts(int id)
        {
            return ProductCategorysRepository.GetCategoryAndItsProductsById(id);
        }

        public void SaveProductCategory()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
