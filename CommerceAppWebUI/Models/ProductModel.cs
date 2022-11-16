using CommerceApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace CommerceAppWebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Required]
        [MinLength(5),MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(5), MaxLength(100)]
        public string Url { get; set; }

        [Required]
        [Range(1,50000)]
        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        //[Required]
        public string? ImgUrl { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsHomePage { get; set; } = false;
        public List<Category>? SelectedCategories { get; set; }
    }
}
