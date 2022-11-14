using CommerceApp.Data.Abstract;
using CommerceApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, CommerceAppContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId)
        {
            using (var context = new CommerceAppContext())
            {
                var orders = context.Orders.Include(i => i.OrderItems)
                                    .ThenInclude(i => i.Product)
                                    .AsQueryable();

                if(!string.IsNullOrEmpty(userId))
                {
                    orders=orders.Where(i => i.UserId == userId);
                }
                return orders.ToList();
            }
        }
    }
}
