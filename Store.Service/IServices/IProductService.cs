using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface IProductService
    {
        Product GetProduct(int id);
        List<Product> GetProducts(string name);
        List<Product> GetProductsByName(string name);
        List<Product> GetNewProducts();
        List<SizeAndProductId> GetProductSizes(string Name);
        List<Product> GetRelatedItems(int Id);
        void CreateProduct(Product Product);
        void RemoveProduct(int id);
        void UpdateProduct(Product Product);
        void SaveProduct();
    }
}
