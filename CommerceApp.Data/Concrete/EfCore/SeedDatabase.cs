using CommerceApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Concrete.EfCore
{
    public static class SeedDatabase
    {
       public static void Seed()
       {
            var context = new CommerceAppContext();
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }
                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                }
                context.AddRange(ProductCategories);
            }
            context.SaveChanges();
        }

        private static Category[] Categories =
        {
            new Category(){Name="Phone",Url="phone"},
            new Category(){Name="Computer",Url="computer"},
            new Category(){Name="Electronic",Url="electronic"},
            new Category(){Name="White Goods",Url="white-goods"}
        };

        private static Product[] Products =
        {
            new Product(){Name="Iphone X",Url="iphone-x",Price=1000,Description="Good Devices",ImgUrl="iphoneX.jpeg",IsApproved=true},
            new Product(){Name="Iphone XR",Url="iphone-xr",Price=2000,Description="Good Devices",ImgUrl="iphoneXR.jpg",IsApproved=true},
            new Product(){Name="Iphone 11",Url="iphone-11",Price=3000,Description="Good Devices",ImgUrl="iphone11.jpg",IsApproved=true},
            new Product(){Name="Iphone 12",Url="iphone-12",Price=4000,Description="Good Devices",ImgUrl="iphone12.jpg",IsApproved=true},
            new Product(){Name="Iphone 13",Url="iphone-13",Price=5000,Description="Good Devices",ImgUrl="iphone13.png",IsApproved=true}
        };
        private static ProductCategory[] ProductCategories =
        {
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[2]},
            new ProductCategory(){Product=Products[2],Category=Categories[0]},
            new ProductCategory(){Product=Products[2],Category=Categories[2]},
            new ProductCategory(){Product=Products[3],Category=Categories[0]},
            new ProductCategory(){Product=Products[3],Category=Categories[2]},
            new ProductCategory(){Product=Products[4],Category=Categories[0]},
            new ProductCategory(){Product=Products[4],Category=Categories[2]},
        };
    }
}
