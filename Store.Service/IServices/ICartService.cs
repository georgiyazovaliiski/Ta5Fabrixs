using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface ICartService
    {
        Cart GetCart(string id);
        void CreateCart(Cart Cart);
        void RemoveCart(int id);
        void SaveCart();
    }
}
