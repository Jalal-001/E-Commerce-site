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
        public void AddToCart(string userid, int productid, int quantity)
        {
            var cart = getCartByUserId(userid);
            if (cart != null)
            {
                var index = cart.CartItems.FindIndex(i => i.ProductId == productid);
                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = productid,
                        Quantity = quantity
                    });
                }
                else
                    cart.CartItems[index].Quantity += quantity;

                Update(cart);
            }
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            using (var context = new CommerceAppContext())
            {
                var cmd = "Delete from CartItems where CartId=@p0 and ProductId=@p1";
                context.Database.ExecuteSqlRaw(cmd,cartId,productId);
            }
        }

        public Cart getCartByUserId(string userId)
        {
            using (var context = new CommerceAppContext())
            {
                return context.Carts
                    .Include(a => a.CartItems)
                    .ThenInclude(a => a.Product)
                    .FirstOrDefault(a => a.UserId == userId);
            }
        }
        public override void Update(Cart entity)
        {
            using (var context = new CommerceAppContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }
    }
}
