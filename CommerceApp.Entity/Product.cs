using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsHomePage { get; set; } = false;
        public List<ProductCategory> ProductCategories { get; set; }

    }
}
