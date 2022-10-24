using CommerceApp.Data.Abstract;
using CommerceApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, CommerceAppContext>, ICategoryRepository
    {
        public void DeleteProductFromCategory(int categoryId, int productId)
        {
            using(var context=new CommerceAppContext())
            {
                var msg = "delete from ProductCategory where CategoryId=@p0 and ProductId=@p1";
                context.Database.ExecuteSqlRaw(msg,categoryId,productId);
            }
        }

        public Category getByIdWithProducts(int categoryId)
        {
            using(var context=new CommerceAppContext())
            {
                return context.Categories
                    .Where(c => c.CategoryId == categoryId)
                    .Include(c => c.ProductCategories)
                    .ThenInclude(c => c.Product)
                    .FirstOrDefault();
            }
        }
    }
}
