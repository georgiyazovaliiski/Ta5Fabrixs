using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface IPromoService
    {
        Promo GetPromo(int id);
        List<Promo> GetPromos();
        List<Promo> GetEndingPromos();
        void UpdateAllPromos();
        void CreatePromo(Promo Promo);
        void RemovePromo(int id);
        void SavePromo();
    }
}
