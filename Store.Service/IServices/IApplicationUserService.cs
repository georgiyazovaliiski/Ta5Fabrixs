using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface IApplicationUserService
    {
        ApplicationUser GetApplicationUser(string id);
        void CreateApplicationUser(ApplicationUser ApplicationUser);
        void UpdateApplicationUser(ApplicationUser ApplicationUser);
        void RemoveApplicationUser(string id);
        void AddMail(string mailName);
        void SaveApplicationUser();
    }
}
