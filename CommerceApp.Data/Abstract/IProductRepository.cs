using CommerceApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        List<Product> GetSearchResult(string searchString);
        Product GetProductDetails(string url);
        Product GetById(int id);
        List<Product>GetProductByCategory(string categoryName,int page, int pageSize);
        int getCountByCategory(string category);
        List<Product> GetHomePageProducts();
        Product GetByIdWithCategories(int productId);
        void Update(Product entity,int[] categoryIds);


    }
}
