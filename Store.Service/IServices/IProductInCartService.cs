using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface IProductInCartService
    {
        ProductInCart GetProductInCart(int id);
        void CreateProductInCart(ProductInCart ProductInCart);
        void UpdateProductInCart(ProductInCart ProductInCart);
        void RemoveProductInCart(int id);
        void SaveProductInCart();
    }
}
