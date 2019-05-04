using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface ILayoutService
    {
        LayoutModel GetLayout(int id);
        void CreateLayout(LayoutModel Layout);
        void UpdateLayout(LayoutModel Layout);
        void SaveLayout();
    }
}
