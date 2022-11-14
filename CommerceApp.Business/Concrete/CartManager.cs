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
        private readonly IUnitOfWork _unitOfWork;
        public CartManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddToCart(string userid, int productid, int quantity)
        {
            _unitOfWork.Carts.AddToCart(userid, productid, quantity);
        }

        public void ClearCart(int cartId)
        {
            _unitOfWork.Carts.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            // Elaqeli cedvelden melumati silmek ucun ilk once cart tapilmalidir.
            // sonra cart-in id-si ve product-in id-si verilerek silinir.
            var cart = getCartByUserId(userId);
            if (cart != null)
            {
                _unitOfWork.Carts.DeleteFromCart(cart.Id, productId);
            }
        }

        public Cart getCartByUserId(string userId)
        {
            return _unitOfWork.Carts.getCartByUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            _unitOfWork.Carts.Create(new Cart { UserId = userId });
            _unitOfWork.Save();
        }
    }
}
