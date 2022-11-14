using CommerceApp.Business.Abstract;
using CommerceApp.Data.Abstract;
using CommerceApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void Create(Order order)
        {
            _unitOfWork.Orders.Create(order);
            _unitOfWork.Save();
        }

        public List<Order> GetOrders(string userId)
        {
            return _unitOfWork.Orders.GetOrders(userId);
        }
    }
}
