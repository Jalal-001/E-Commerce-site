using CommerceApp.Business.Abstract;
using CommerceApp.Entity;
using CommerceAppWebUI.Extensions;
using CommerceAppWebUI.Identity;
using CommerceAppWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CommerceAppWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(a => a.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/Admin/user/list");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);

                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());

                        return Redirect("/Admin/user/list");

                        TempData.Put("message", new AlertMessage()
                        {
                            AlertType = "success",
                            Title = "Edit",
                            Message = "Successfully Updated"
                        });
                    }
                }
                return Redirect("/Admin/user/list");
            }

            return View(model);
        }

        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "Create Error",
                Message = "An unknown error occurred!"
            });
            return View(model);
        }

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonMembers = new List<User>();

            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name)
                    ? members : nonMembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            foreach (var userId in model.IdsToAdd ?? new string[] { })
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }

            foreach (var userId in model.IdsToDelete ?? new string[] { })
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }

            return Redirect("/admin/role/" + model.RoleId);
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "",
                Message = "Error"
            });
        }

        public IActionResult ProductList()
        {
            var productListWiewModel = new ProductListViewModel()
            {
                Products = _productService.GetAll()
            };
            return View(productListWiewModel);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Price = model.Price,
                    Description = model.Description,
                    ImgUrl = model.ImgUrl
                };
                _productService.Create(entity);

                return RedirectToAction("ProductList");

                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "success",
                    Title = "Succesfully created",
                    Message = $"{entity.Name} succesfully created"
                });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
                return NotFound();

            //entity -> from database
            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
                return NotFound();

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                Description = entity.Description,
                ImgUrl = entity.ImgUrl,

                SelectedCategories = entity.ProductCategories.Select(o => o.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //model -> from 'UpdateProduct' form

                var entity = _productService.GetById(model.ProductId);
                if (entity == null)
                    return NotFound();

                entity.ProductId = model.ProductId;
                entity.Name = model.Name;
                entity.Url = model.Url;
                entity.Price = model.Price;
                entity.Description = model.Description;
                entity.IsHomePage = model.IsHomePage;
                entity.IsApproved = model.IsApproved;

                if (file != null)
                {
                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    entity.ImgUrl = randomName;
                }

                _productService.Update(entity, categoryIds);

                // Alert Message
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "success",
                    Title = "Succesfully updated",
                    Message = $"{entity.Name} succesfully updated"
                });
                return RedirectToAction("ProductList");
            }
            return View(model);

        }
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity == null)
                return NotFound();
            _productService.Delete(entity);

            // Alert Message
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "warning",
                Title = "Succesfully updated",
                Message = $"{entity.Name} succesfully deleted"
            });
            return RedirectToAction("ProductList");
        }


        // *************** Category Operations ***************

        public IActionResult CategoryList()
        {
            var categoryListViewModel = new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            };
            return View(categoryListViewModel);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Url = model.Url
                };
                _categoryService.Create(category);

                // Alert Message
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "success",
                    Title = "Succesfully created",
                    Message = $"{category.Name} succesfully created"
                });
                return RedirectToAction("CategoryList");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
                return NotFound();

            var category = _categoryService.getByIdWithProducts((int)id);

            if (category == null)
                return NotFound();

            var model = new CategoryModel()
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Url = category.Url,
                Products = category.ProductCategories.Select(p => p.Product).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.CategoryId);

                if (entity == null)
                    return NotFound();

                entity.CategoryId = model.CategoryId;
                entity.Name = model.Name;
                entity.Url = model.Url;

                _categoryService.Update(entity);

                // Alert Message
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "success",
                    Title = "Succesfully updated",
                    Message = $"{entity.Name} succesfully updated"
                });
                return RedirectToAction("CategoryList");
            }
            return View(model);
        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
                return BadRequest();

            _categoryService.Delete(category);

            // Alert Message
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "warning",
                Title = "Succesfully deleted",
                Message = $"{category.Name} succesfully deleted"
            });
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteProductFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteProductFromCategory(categoryId, productId);
            return Redirect("/Admin/Categories" + categoryId);
        }



    }
}
