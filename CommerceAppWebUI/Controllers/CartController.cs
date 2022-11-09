using CommerceApp.Business.Abstract;
using CommerceAppWebUI.Identity;
using CommerceAppWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CommerceAppWebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private UserManager<User> _userManager;

        public CartController(ICartService cartService, UserManager<User> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = _cartService.getCartByUserId(_userManager.GetUserId(User));

            return View(new CartModel
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImgUrl = i.Product.ImgUrl,
                    Quantity = i.Quantity

                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart()
        {
            return View();
        }
    }
}
