using System.ComponentModel.DataAnnotations;

namespace CommerceAppWebUI.Models
{
    public class OrderModel
    {
        //public int OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Note { get; set; }
        public string CardName { get; set; }

        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }

        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvc { get; set; }
        public CartModel CartModel { get; set; }
    }
}
