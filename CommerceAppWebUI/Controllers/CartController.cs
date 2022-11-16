using CommerceApp.Business.Abstract;
using CommerceApp.Entity;
using CommerceAppWebUI.Extensions;
using CommerceAppWebUI.Identity;
using CommerceAppWebUI.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CommerceAppWebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IOrderService _orderService;
        private UserManager<User> _userManager;

        public CartController(ICartService cartService, UserManager<User> userManager, IOrderService orderService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _orderService = orderService;
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
        public IActionResult AddToCart(int productid, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.AddToCart(userId, productid, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.DeleteFromCart(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = _cartService.getCartByUserId(_userManager.GetUserId(User));
            var orderModel = new OrderModel()
            {
                CartModel = new CartModel
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
                }
            };
            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            var userId = _userManager.GetUserId(User);
            var cart = _cartService.getCartByUserId(userId);

            model.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImgUrl = i.Product.ImgUrl,
                    Quantity = i.Quantity
                }).ToList()
            };

            var payment = PaymentProcess(model);

            if (payment.Status == "success")
            {
                SaveOrder(model, payment, userId);
                ClearCart(model.CartModel.CartId);
                return View("Success");
            }
            else
            {
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "danger",
                    Title = "Error",
                    Message = payment.ErrorMessage
                });
            }
            return View(model);
        }

        // Clear cart after order
        private void ClearCart(int cartId)
        {
            _cartService.ClearCart(cartId);
        }

        // Save order in DB
        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = EnumOrderState.complated;
            order.EnumPaymentType = EnumPaymentType.CreditCard;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UserId = userId;
            order.Address = model.Address;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;
            order.Note = "";

            order.OrderItems = new List<CommerceApp.Entity.OrderItem>();

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new CommerceApp.Entity.OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderItem);
            }
            _orderService.Create(order);
        }

        // Payment
        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-M0weXOlK7ssCZDzRFrYka27HH0PYc5n3";
            options.SecretKey = "sandbox-tfesixKQNtgkz3XDkR3fxCMEbAB9mi6M";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111, 999999999).ToString();
            request.Price = model.CartModel.TotalPrice().ToString();
            request.PaidPrice = model.CartModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = "Ali";
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvc;
            paymentCard.RegisterCard = 0;

            request.PaymentCard = paymentCard;

            //paymentCard.CardNumber = "5528790000000008";
            //paymentCard.ExpireMonth = "12";
            //paymentCard.ExpireYear = "2030";
            //paymentCard.Cvc = "123";

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Telefon";
                basketItem.Price = (item.Price * item.Quantity).ToString();
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItems.Add(basketItem);
            }
            request.BasketItems = basketItems;
            return Payment.Create(request, options);
        }
    }
}
