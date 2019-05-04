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
    public class ImageService : IImageService
    {
        private readonly IImageRepository ImageRepository;
        private readonly IUnitOfWork unitOfWork;

        public ImageService(IImageRepository ImageRepository, IUnitOfWork unitOfWork)
        {
            this.ImageRepository = ImageRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IImageService Members

        public Image GetImage(int id)
        {
            var Image = ImageRepository.GetById(id);
            return Image;
        }

        public void CreateImage(Image Image)
        {
            ImageRepository.Add(Image);
        }

        public string RemoveImage(int Id)
        {
            var Image = ImageRepository.GetById(Id);
            var url = Image.UrlName;
            ImageRepository.Delete(Image);
            return url;
        }

        public void SaveImage()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
