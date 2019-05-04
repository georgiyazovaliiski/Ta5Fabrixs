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
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IPromoCodeRepository PromoCodeRepository;
        private readonly IUnitOfWork unitOfWork;

        public PromoCodeService(IPromoCodeRepository PromoCodeRepository, IUnitOfWork unitOfWork)
        {
            this.PromoCodeRepository = PromoCodeRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IPromoCodeService Members

        public PromoCode GetPromoCode(int id)
        {
            var PromoCode = PromoCodeRepository.GetById(id);
            return PromoCode;
        }

        public PromoCode GetPromoCodeByName(string Name)
        {
            var PromoCode = PromoCodeRepository.GetByName(Name);
            return PromoCode;
        }

        public void CreatePromoCode(PromoCode PromoCode)
        {
            PromoCodeRepository.Add(PromoCode);
        }

        public void RemovePromoCode(int Id)
        {
            var PromoCode = PromoCodeRepository.GetById(Id);
            PromoCodeRepository.Delete(PromoCode);
        }

        public void SavePromoCode()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
