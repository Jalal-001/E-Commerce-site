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
        private ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void Create(Category entity)
        {
            _categoryRepository.Create(entity);
        }

        public void Delete(Category entity)
        {
            _categoryRepository.Delete(entity);
        }

        public void DeleteProductFromCategory(int categoryId, int productId)
        {
            _categoryRepository.DeleteProductFromCategory(categoryId, productId);
        }

        public List<Category> GetAll()
        {
           return _categoryRepository.GetAll();
        }

        public Category GetById(int id)
        {
          return _categoryRepository.GetById(id);
        }

        public Category getByIdWithProducts(int categoryId)
        {
            return _categoryRepository.getByIdWithProducts(categoryId);
        }

        public void Update(Category entity)
        {
           _categoryRepository.Update(entity);
        }
    }
}
