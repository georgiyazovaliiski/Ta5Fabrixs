using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface IUserDetailsService
    {
        UserDetails GetUserDetails(int id);
        UserDetails GetUserDetails(string UserId);
        void CreateUserDetails(UserDetails UserDetails);
        void RemoveUserDetails(int id);
        void SaveUserDetails();
    }
}
