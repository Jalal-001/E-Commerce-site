using CommerceAppWebUI.Models;
using Microsoft.AspNetCore.Mvc;
using CommerceApp.Data.Abstract;
using CommerceApp.Business.Abstract;

namespace CommerceAppWebUI.Controllers
{
    public class HomeController:Controller
    {
        private IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService; 
        }



        public ActionResult index()
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productService.GetHomePageProducts()  
            };

            return View(productListViewModel);
        }
        public ActionResult about()
        {
            return View();
        }
    }
}
