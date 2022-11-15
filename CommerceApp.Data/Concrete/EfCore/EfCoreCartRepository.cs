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
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart>, ICartRepository
    {
        public EfCoreCartRepository(CommerceAppContext context) : base(context)
        {

        }
        private CommerceAppContext context
        {
            get { return (CommerceAppContext)base.context; }
        }

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

        public override void Update(Cart entity)
        {
            context.Carts.Update(entity);
            context.SaveChanges();
            // Burada Update override olundugu ucun 'SaveChanges' xususile cagirilmalidir.
            // Digerlerinde ise '_UnitOfWork.Save()'
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            var cmd = "Delete from CartItems where CartId=@p0 and ProductId=@p1";
            context.Database.ExecuteSqlRaw(cmd, cartId, productId);
        }

        public Cart getCartByUserId(string userId)
        {
            return context.Carts
                .Include(a => a.CartItems)
                .ThenInclude(a => a.Product)
                .FirstOrDefault(a => a.UserId == userId);
        }

        public void ClearCart(int cartId)
        {
            var cmd = "Delete from CartItems where CartId=@p0";
            context.Database.ExecuteSqlRaw(cmd, cartId);
        }
    }
}
