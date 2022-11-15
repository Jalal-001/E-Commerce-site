using CommerceApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Configurations
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Product>().HasData
            (
                new Product() { ProductId = 1, Name = "Iphone X", Url = "iphone-x", Price = 1000, Description = "Good Devices", ImgUrl = "iphoneX.jpeg", IsApproved = true, IsHomePage = true },
                new Product() { ProductId = 2, Name = "Iphone XR", Url = "iphone-xr", Price = 1200, Description = "Good Devices", ImgUrl = "iphoneXR.jpg", IsApproved = true, IsHomePage = true },
                new Product() { ProductId = 3, Name = "Iphone 12", Url = "iphone-12", Price = 1500, Description = "Good Devices", ImgUrl = "iphone12.jpg", IsApproved = true, IsHomePage = true },
                new Product() { ProductId = 4, Name = "Iphone 13", Url = "iphone-13", Price = 1700, Description = "Good Devices", ImgUrl = "iphone13.png", IsApproved = true, IsHomePage = true },
                new Product() { ProductId = 5, Name = "Lenova Idepad 3", Url = "lenova-idepad-3", Price = 1300, Description = "Good Devices", ImgUrl = "lenova1.jpg", IsApproved = true, IsHomePage = true },
                new Product() { ProductId = 6, Name = "Lenova Idepad 5", Url = "lenova-idepad-5", Price = 1550, Description = "Good Devices", ImgUrl = "lenova1.jpg", IsApproved = true, IsHomePage = true }
            );

            builder.Entity<Category>().HasData
            (
                new Category() { CategoryId = 1, Name = "Phone", Url = "phone" },
                new Category() { CategoryId = 2, Name = "Computer", Url = "computer" },
                new Category() { CategoryId = 3, Name = "Electronic", Url = "electronic" },
                new Category() { CategoryId = 4, Name = "Tv & Audio ", Url = "tv-audio" }
            );

            builder.Entity<ProductCategory>().HasData
            (
                new ProductCategory() { ProductId = 1, CategoryId = 1 },
                new ProductCategory() { ProductId = 1, CategoryId = 3 },
                new ProductCategory() { ProductId = 2, CategoryId = 1 },
                new ProductCategory() { ProductId = 2, CategoryId = 3 },
                new ProductCategory() { ProductId = 3, CategoryId = 1 },
                new ProductCategory() { ProductId = 3, CategoryId = 3 },
                new ProductCategory() { ProductId = 4, CategoryId = 1 },
                new ProductCategory() { ProductId = 4, CategoryId = 3 },
                new ProductCategory() { ProductId = 5, CategoryId = 2 },
                new ProductCategory() { ProductId = 5, CategoryId = 3 },
                new ProductCategory() { ProductId = 6, CategoryId = 2 },
                new ProductCategory() { ProductId = 6, CategoryId = 3 }
            );
        }
    }
}
