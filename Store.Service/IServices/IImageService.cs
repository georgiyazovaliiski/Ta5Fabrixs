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
    public interface IImageService
    {
        Image GetImage(int id);
        void CreateImage(Image Image);
        string RemoveImage(int id);
        void SaveImage();
    }
}
