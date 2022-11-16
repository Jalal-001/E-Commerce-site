using CommerceApp.Data.Abstract;
using CommerceApp.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product>, IProductRepository
    {
        public EfCoreProductRepository(CommerceAppContext context) : base(context)
        {

        }
        private CommerceAppContext context
        {
            get { return (CommerceAppContext)base.context; }
        }


        public Product GetByIdWithCategories(int productId)
        {

            return context.Products
                .Where(p => p.ProductId == productId)
                .Include(p => p.ProductCategories)
                .ThenInclude(p => p.Category)
                .FirstOrDefault();
        }

        public int getCountByCategory(string category)
        {

            var products = context.Products.Where(a => a.IsApproved).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products
                .Include(p => p.ProductCategories)
                .ThenInclude(p => p.Category)
                .Where(p => p.ProductCategories.Any(a => a.Category.Url == category));
            }
            return products.Count();
        }

        public List<Product> GetHomePageProducts()
        {

            return context.Products.Where(i => i.IsApproved && i.IsHomePage).ToList();
        }

        public List<Product> GetProductByCategory(string categoryName, int page, int pageSize)
        {

            var products = context.Products.Where(a => a.IsApproved).AsQueryable();

            if (!string.IsNullOrEmpty(categoryName))
            {
                products = products
                .Include(p => p.ProductCategories)
                .ThenInclude(p => p.Category)
                .Where(p => p.ProductCategories.Any(a => a.Category.Url == categoryName));
            }
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public Product GetProductDetails(string url)
        {

            return context.Products
                .Where(i => i.Url == url)
                .Include(i => i.ProductCategories)
                .ThenInclude(i => i.Category)
                .FirstOrDefault();
        }

        public List<Product> GetSearchResult(string searchString)
        {

            var products = context.Products.Where(p => p.IsApproved).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products
                   .Where(a => a.Name.ToLower().Contains(searchString) || a.Description.ToLower().Contains(searchString));
            }
            return products.ToList();
        }

        public void Update(Product entity, int[] categoryIds)
        {

            var product = context.Products
                .Include(l => l.ProductCategories)
                .FirstOrDefault(l => l.ProductId == entity.ProductId);

            if (product != null)
            {
                product.ProductId = entity.ProductId;
                product.Name = entity.Name;
                product.Price = entity.Price;
                product.Description = entity.Description;
                product.Url = entity.Url;
                product.ImgUrl = entity.ImgUrl;
                product.IsApproved = entity.IsApproved;
                product.IsHomePage = entity.IsHomePage;


                product.ProductCategories = categoryIds.Select(CatId => new ProductCategory()
                {
                    ProductId = entity.ProductId,
                    CategoryId = CatId,

                }).ToList();
            }
        }

        //public Product GetById(int id)
        //{
        //    using(var context=new CommerceAppContext())
        //    {
        //       var product= context.Products.Where(a => a.ProductId == id).FirstOrDefault();
        //       return product;
        //    }
        //}

    }
}
