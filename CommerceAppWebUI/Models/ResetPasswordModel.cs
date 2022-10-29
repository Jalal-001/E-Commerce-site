using System.ComponentModel.DataAnnotations;

namespace CommerceAppWebUI.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Token { get; set; }

    }
}
