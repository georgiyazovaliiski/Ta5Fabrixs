using Store.Model.Models;
using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface IPromoCodeService
    {
        PromoCode GetPromoCode(int id);
        PromoCode GetPromoCodeByName(string Name);
        void CreatePromoCode(PromoCode PromoCode);
        void RemovePromoCode(int id);
        void SavePromoCode();
    }
}
