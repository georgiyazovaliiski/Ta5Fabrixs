using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface ICookieCartService
    {
        CookieCart GetCookieCart(string id);
        void CreateCookieCart(CookieCart CookieCart);
        void UpdateCookieCart(CookieCart CookieCart);
        void RemoveCookieCart(int id);
        void SaveCookieCart();
    }
}
