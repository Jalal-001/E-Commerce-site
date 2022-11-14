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
    public class CategoryManager : ICategoryService
    {
        // inject
        //private ICategoryRepository _categoryRepository;
        //public CategoryManager(ICategoryRepository categoryRepository)
        //{
        //    _categoryRepository = categoryRepository;
        //}

        // or
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(Category entity)
        {
            _unitOfWork.Categories.Create(entity);
            _unitOfWork.Save();
        }

        public void Delete(Category entity)
        {
            _unitOfWork.Categories.Delete(entity);
            _unitOfWork.Save();
        }

        public void DeleteProductFromCategory(int categoryId, int productId)
        {
            _unitOfWork.Categories.DeleteProductFromCategory(categoryId, productId);
        }

        public List<Category> GetAll()
        {
           return _unitOfWork.Categories.GetAll();
        }

        public Category GetById(int id)
        {
          return _unitOfWork.Categories.GetById(id);
        }

        public Category getByIdWithProducts(int categoryId)
        {
            return _unitOfWork.Categories.getByIdWithProducts(categoryId);
        }

        public void Update(Category entity)
        {
            _unitOfWork.Categories.Update(entity);
            _unitOfWork.Save();
        }
    }
}
