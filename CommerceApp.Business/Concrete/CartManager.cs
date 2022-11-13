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
    public class CartManager : ICartService
    {
        private ICartRepository _cartRepository;
        public CartManager(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public void AddToCart(string userid, int productid, int quantity)
        {
            _cartRepository.AddToCart(userid, productid, quantity);
        }

        public void ClearCart(int cartId)
        {
            _cartRepository.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            // Elaqeli cedvelden melumati silmek ucun ilk once cart tapilmalidir.
            // sonra cart-in id-si ve product-in id-si verilerek silinir.
            var cart = getCartByUserId(userId);
            if (cart != null)
            {
                _cartRepository.DeleteFromCart(cart.Id, productId);
            }
        }

        public Cart getCartByUserId(string userId)
        {
            return _cartRepository.getCartByUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            _cartRepository.Create(new Cart { UserId = userId });
        }
    }
}
