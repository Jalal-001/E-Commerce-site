using CommerceApp.Entity;

namespace CommerceAppWebUI.Models
{
    public class OrderListModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public EnumOrderState OrderState { get; set; }
        public EnumPaymentType EnumPaymentType { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }

        public double TotalPrice()
        {
            return OrderItems.Sum(p => p.Price * p.Quantity);
        }
    }
    public class OrderItemModel
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
