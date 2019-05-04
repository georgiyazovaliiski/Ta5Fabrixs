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
    public class PromoService : IPromoService
    {
        private readonly IPromoRepository PromosRepository;
        private readonly IUnitOfWork unitOfWork;

        public PromoService(IPromoRepository PromosRepository, IUnitOfWork unitOfWork)
        {
            this.PromosRepository = PromosRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IPromoService Members

        public void UpdateAllPromos()
        {
            var Promos = PromosRepository.GetAll();
            DateTime Now = DateTime.Now;
            foreach (var promo in Promos)
            {
                if(promo.EndDate <= Now)
                {
                    promo.Product.PriceEU = promo.Product.OriginalPriceEU;
                    promo.Product.TagsText = "Ta5";
                    PromosRepository.Update(promo);
                    PromosRepository.Delete(promo);
                }
                else if (promo.StartDate < Now && promo.Active == false)
                {
                    promo.Active = true;
                    PromosRepository.Update(promo);
                }
            }
        }

        public Promo GetPromo(int id)
        {
            var Promo = PromosRepository.GetById(id);
            return Promo;
        }

        public void CreatePromo(Promo Promo)
        {
            PromosRepository.Add(Promo);
        }

        public void RemovePromo(int Id)
        {
            var Promo = PromosRepository.GetById(Id);
            PromosRepository.Delete(Promo);
        }

        public List<Promo> GetPromos()
        {
            return PromosRepository.GetAll().ToList();
        }

        public List<Promo> GetEndingPromos()
        {
            return PromosRepository.GetEndingPromos();
        }

        public void SavePromo()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
