using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface IOrderService
    {
        Order GetOrder(int id);
        List<Order> GetAll();
        void CreateOrder(Order Order);
        void RemoveOrder(Order Order);
        void UpdateOrder(Order Order);
        void SaveOrder();
    }
}
