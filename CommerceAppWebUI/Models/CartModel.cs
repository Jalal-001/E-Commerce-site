namespace CommerceAppWebUI.Models
{
    public class CartModel
    {
        public int CartId { get; set; }
        public List<CartItemModel> CartItems { get; set; }
        public Double TotalPrice()
        {
            return CartItems.Sum(r => r.Quantity * r.Price);
        }
    }

    public class CartItemModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImgUrl { get; set; }
        public int Quantity { get; set; }
    }
}
