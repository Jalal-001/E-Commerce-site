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
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart, CommerceAppContext>, ICartRepository
    {
        public Cart getCartByUserId(string userId)
        {
            using (var context=new CommerceAppContext())
            {
                return context.Carts
                    .Include(a => a.CartItems)
                    .ThenInclude(a => a.Product)
                    .FirstOrDefault(a => a.UserId == userId);
            }
        }
    }
}
