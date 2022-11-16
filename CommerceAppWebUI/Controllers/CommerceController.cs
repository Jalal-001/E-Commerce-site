using CommerceApp.Business.Abstract;
using CommerceApp.Entity;
using CommerceAppWebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommerceAppWebUI.Controllers
{
    public class CommerceController:Controller
    {
        private IProductService _productService;

        public CommerceController(IProductService productService)
        {
            _productService = productService;
        }



        public IActionResult list(string category,int page=1)
        {
            const int pageSize = 3;
            var productListViewModel = new ProductListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.getCountByCategory(category),
                    ItemsPerPage = pageSize,
                    CurrentPage = page,
                    CurrentCategory = category
                },
                Products = _productService.GetproductByCategory(category,page,pageSize),
            };
            return View(productListViewModel);
        }

        public IActionResult Details(string url)
        {
            if (url == null)
                return NotFound();

            Product product = _productService.GetProductDetails(url);

            if (product == null)
                return NotFound();

            return View(new ProductDetailModel
            {
                Product = product,
                Categories = product.ProductCategories.Select(c => c.Category).ToList()
            });
        }

        public IActionResult Search(string q)
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productService.GetSearchResult(q)
            };
            return View(productListViewModel);
        }
    }
}
