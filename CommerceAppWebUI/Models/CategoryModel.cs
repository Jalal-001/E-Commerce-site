using CommerceApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace CommerceAppWebUI.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required]
        [MinLength(5), MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MinLength(5), MaxLength(60)]
        public string Url { get; set; }
        public List<Product>? Products { get; set; }
    }
}
