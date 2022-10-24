using CommerceApp.Data.Abstract;
using CommerceApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, CommerceAppContext>, IOrderRepository
    {
        public List<Order> GetPopularOrders()
        {
            throw new NotImplementedException();
        }
    }
}
