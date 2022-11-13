using CommerceApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Business.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string userId);
        Cart getCartByUserId(string userId);
        void AddToCart(string userid, int productid, int quantity);
        void DeleteFromCart(string userId, int productId);
        void ClearCart(int cartId);
    }
}
