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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository OrdersRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IOrderRepository OrdersRepository, IUnitOfWork unitOfWork)
        {
            this.OrdersRepository = OrdersRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IOrderService Members

        public List<Order> GetAll()
        {
            return this.OrdersRepository.GetAll().ToList();
        }

        public Order GetOrder(int id)
        {
            var Order = OrdersRepository.GetById(id);
            return Order;
        }

        public void CreateOrder(Order Order)
        {
            OrdersRepository.Add(Order);
        }

        public void UpdateOrder(Order Order)
        {
            OrdersRepository.Update(Order);
        }

        public void SaveOrder()
        {
            unitOfWork.Commit();
        }

        public void RemoveOrder(Order Order)
        {
            OrdersRepository.Delete(Order);
        }

        #endregion
    }
}
