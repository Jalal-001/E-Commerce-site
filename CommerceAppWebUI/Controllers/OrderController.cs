using CommerceApp.Business.Abstract;
using CommerceApp.Business.Concrete;
using CommerceAppWebUI.Identity;
using CommerceAppWebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CommerceAppWebUI.Controllers
{
    public class OrderController:Controller
    {
        private UserManager<User> _userManager;
        private IOrderService _orderService;
        public OrderController(UserManager<User> userManager, IOrderService orderService)
        {
            _userManager = userManager;
            _orderService = orderService;
        }

        public IActionResult GetOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrders(userId);

            var OrderList = new List<OrderListModel>();
            OrderListModel orderListModel;

            foreach (var order in orders)
            {
                orderListModel = new OrderListModel();

                orderListModel.Id = order.Id;
                orderListModel.OrderNumber = order.OrderNumber;
                orderListModel.OrderDate = order.OrderDate;
                orderListModel.UserId = order.UserId;
                orderListModel.FirstName = order.FirstName;
                orderListModel.LastName = order.LastName;
                orderListModel.Address = order.Address;
                orderListModel.City = order.City;
                orderListModel.Phone = order.Phone;
                orderListModel.Email = order.Email;
                orderListModel.Note = order.Note;
                orderListModel.OrderState = order.OrderState;
                orderListModel.EnumPaymentType = order.EnumPaymentType;

                orderListModel.OrderItems = order.OrderItems.Select(o => new OrderItemModel()
                {
                    OrderItemId=o.Id,
                    OrderId=o.OrderId,
                    ProductId=o.ProductId,
                    Name=o.Product.Name,
                    ImgUrl=o.Product.ImgUrl,
                    Price=o.Price,
                    Quantity=o.Quantity

                }).ToList();

                OrderList.Add(orderListModel);
            }
            return View("Orders", OrderList);
        }
    }
}
