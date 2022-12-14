using CommerceApp.Data.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using CommerceApp.Data.Concrete.EfCore;
using CommerceApp.Business.Abstract;
using CommerceApp.Business.Concrete;
using CommerceAppWebUI.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using CommerceAppWebUI.EmailServices;
using CommerceAppWebUI.Extensions;

var builder = WebApplication.CreateBuilder(args);



IConfiguration _configuration;
_configuration = builder.Configuration;


// *** Add services to the container (For MVC) ***


//builder.Services.AddDbContext<CommerceAppContext>(options => options.UseSqlite(_configuration.GetConnectionString("Sqlite")));
//builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(_configuration.GetConnectionString("Sqlite")));

builder.Services.AddDbContext<CommerceAppContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MSSql"), options => options.EnableRetryOnFailure()));
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MSSql"), options => options.EnableRetryOnFailure()));

//builder.Services.AddDbContext<CommerceAppContext>(options => options.UseMySQL(_configuration.GetConnectionString("MySql")));
//builder.Services.AddDbContext<ApplicationContext>(options => options.UseMySQL(_configuration.GetConnectionString("MySql")));


// Identity

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    //password
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;

    //Lockout
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = ".CommerceApp.Security.Cookie"
    };
});

// smtp emailSender
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
                new SmtpEmailSender(
                    _configuration["EmailSender:Host"],
                    _configuration.GetValue<int>("EmailSender:Port"),
                    _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    _configuration["EmailSender:UserName"],
                    _configuration["EmailSender:Password"])
                );


builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICartService, CartManager>();
builder.Services.AddScoped<IOrderService, OrderManager>();

var app = builder.Build();



//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var cartService = scope.ServiceProvider.GetRequiredService<ICartService>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    SeedIdentity.Seed(userManager, roleManager, cartService, configuration).Wait();
}


// *Route*
app.MapControllerRoute
    (
        name: "orders",
        pattern: "orders",
        defaults: new { controller = "Order", action = "GetOrders" }
    );
app.MapControllerRoute
    (
        name: "checkout",
        pattern: "checkout",
        defaults: new { controller = "Cart", action = "Checkout" }
    );
app.MapControllerRoute
    (
        name: "car",
        pattern: "cart",
        defaults: new { controller = "Cart", action = "Index" }
    );
app.MapControllerRoute
    (
        name: "adminuserlist",
        pattern: "admin/user/list",
        defaults: new { controller = "Admin", action = "UserList" }
    );
app.MapControllerRoute
    (
        name: "adminuseredit",
        pattern: "admin/user/{Id?}",
        defaults: new { controller = "Admin", action = "UserEdit" }
    );
app.MapControllerRoute
    (
        name: "adminrolelist",
        pattern: "admin/role/list",
        defaults: new { controller = "Admin", action = "RoleList" }
    );
app.MapControllerRoute
    (
        name: "admincreaterole",
        pattern: "admin/role/create",
        defaults: new { controller = "Admin", action = "RoleCreate" }
    );
app.MapControllerRoute
    (
        name: "adminroleedit",
        pattern: "admin/role/{Id?}",
        defaults: new { controller = "Admin", action = "RoleEdit" }
    );
app.MapControllerRoute
    (
        name: "adminproductlist",
        pattern: "admin/products",
        defaults: new { controller = "Admin", action = "ProductList" }
    );
app.MapControllerRoute
    (
        name: "adminproductlist",
        pattern: "admin/categories",
        defaults: new { controller = "Admin", action = "CategoryList" }
    );

app.MapControllerRoute
    (
        name: "admineditproduct",
        pattern: "admin/products/{id?}",
        defaults: new { controller = "Admin", action = "EditProduct" }
    );
app.MapControllerRoute
    (
        name: "admineditcategory",
        pattern: "admin/categories/{id?}",
        defaults: new { controller = "Admin", action = "EditCategory" }
    );
app.MapControllerRoute
    (
        name: "search",
        pattern: "products",
        defaults: new { controller = "commerce", action = "search" }
    );
app.MapControllerRoute
    (
        name: "productdetails",
        pattern: "{url}",
        defaults: new { controller = "commerce", action = "details" }
    );
app.MapControllerRoute
    (
        name: "products",
        pattern: "products/{category?}",
        defaults: new { controller = "commerce", action = "list" }
    );
app.MapControllerRoute
    (
        name: "default",
        pattern: "{controller=home}/{action=index}/{id?}"
    );




app.UseHttpsRedirection();

app.UseStaticFiles();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, "node_modules")),
//    RequestPath = "/modules"
//});

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.Run();
