using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface IProductCategoryService
    {
        ProductCategory GetProductCategory(int id);
        List<ProductCategory> GetProductCategories();
        List<ProductCategory> GetCategoriesAndProducts();
        ProductCategory GetProductCategory(string name);
        ProductCategory GetCategoryAndItsProducts(string name);
        ProductCategory GetCategoryAndItsProducts(int id);
        void CreateProductCategory(ProductCategory ProductCategory);
        void UpdateProductCategory(ProductCategory ProductCategory);
        void RemoveProductCategory(int id);
        void SaveProductCategory();
    }
}
