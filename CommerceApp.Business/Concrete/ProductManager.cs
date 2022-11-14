using CommerceApp.Business.Abstract;
using CommerceApp.Data.Abstract;
using CommerceApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(Product entity)
        {
            _unitOfWork.Products.Create(entity);
            _unitOfWork.Save();
        }

        public void Delete(Product entity)
        {
            _unitOfWork.Products.Delete(entity);
            _unitOfWork.Save();
        }

        public List<Product> GetAll()
        {
           return _unitOfWork.Products.GetAll();
        }

        public Product GetById(int id)
        {
            return _unitOfWork.Products.GetById(id);
        }

        public Product GetByIdWithCategories(int productId)
        {
            return _unitOfWork.Products.GetByIdWithCategories(productId);
        }

        public int getCountByCategory(string category)
        {
            return _unitOfWork.Products.getCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _unitOfWork.Products.GetHomePageProducts();
        }

        public List<Product> GetproductByCategory(string categoryName, int page, int pageSize)
        {
            return _unitOfWork.Products.GetProductByCategory(categoryName,page,pageSize);
        }

        public Product GetProductDetails(string url)
        {
           return _unitOfWork.Products.GetProductDetails(url);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _unitOfWork.Products.GetSearchResult(searchString);
        }

        public void Update(Product entity)
        {
            _unitOfWork.Products.Update(entity);
            _unitOfWork.Save();
        }

        public void Update(Product entity, int[] categoryIds)
        {
            _unitOfWork.Products.Update(entity, categoryIds);
            _unitOfWork.Save();
        }
    }
}
