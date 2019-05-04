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
    public class LayoutService : ILayoutService
    {
        private readonly ILayoutRepository LayoutsRepository;
        private readonly IUnitOfWork unitOfWork;

        public LayoutService(ILayoutRepository LayoutsRepository, IUnitOfWork unitOfWork)
        {
            this.LayoutsRepository = LayoutsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ILayoutService Members

        public LayoutModel GetLayout(int id)
        {
            var Layout = LayoutsRepository.GetById(id);
            return Layout;
        }

        public void CreateLayout(LayoutModel Layout)
        {
            LayoutsRepository.Add(Layout);
        }

        public void UpdateLayout(LayoutModel Layout)
        {
            LayoutsRepository.Update(Layout);
        }

        public void SaveLayout()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
