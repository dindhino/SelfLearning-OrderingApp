using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderingApp;
using OrderingApp.Controller;
using OrderingApp.Data;
using OrderingApp.EntityClasses;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<OrderingAppDataContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddEntityFrameworkStores<OrderingAppDataContext>();
builder.Services.AddRazorPages();

//builder.Services.AddAuthentication()
//   .AddGoogle(options =>
//   {
//       IConfigurationSection googleAuthNSection =
//       config.GetSection("Authentication:Google");
//       options.ClientId = googleAuthNSection["ClientId"];
//       options.ClientSecret = googleAuthNSection["ClientSecret"];
//   })
//   .AddMicrosoftAccount(microsoftOptions =>
//   {
//       microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
//       microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
//   });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

TestAPI(connectionString, app);

app.Run();

static void TestAPI(string connectionString, WebApplication app)
{
    OrderingAppDataContext dbContext = new OrderingAppDataContext(new DbContextOptionsBuilder<OrderingAppDataContext>().UseSqlServer(connectionString).Options);
    JustTestAPI_Customer(app, new CustomerController(dbContext));
    JustTestAPI_Product(app, new ProductController(dbContext));
    JustTestAPI_Order(app, new OrderController(dbContext));
}

static void JustTestAPI_Customer(WebApplication app, CustomerController controller)
{
    app.MapGet("/API/Customer/GetAllCustomerList", () => controller.GetAllCustomerList());
    app.MapGet("/API/Customer/GetCustomer", (int? id) => controller.GetCustomer(id));
    app.MapPost("/API/Customer/CreateCustomer", ([FromBody] Customer customer) => controller.CreateCustomer(customer));
    app.MapPut("/API/Customer/UpdateCustomer", ([FromBody] Customer customer, int? id) => controller.UpdateCustomer(customer, id));
    app.MapDelete("/API/Customer/DeleteCustomer", (int? id) => controller.DeleteCustomer(id));
}

static void JustTestAPI_Product(WebApplication app, ProductController controller)
{
    app.MapGet("/API/Product/GetAllProductList", () => controller.GetAllProductList());
    app.MapGet("/API/Product/GetProduct", (int? id) => controller.GetProduct(id));
    app.MapPost("/API/Product/CreateProduct", ([FromBody] Product product) => controller.CreateProduct(product));
    app.MapPut("/API/Product/UpdateProduct", ([FromBody] Product product, int? id) => controller.UpdateProduct(product, id));
    app.MapDelete("/API/Product/DeleteProduct", (int? id) => controller.DeleteProduct(id));
}

static void JustTestAPI_Order(WebApplication app, OrderController controller)
{
    app.MapGet("/API/Order/GetAllOrderList", () => controller.GetAllOrderList());
    app.MapGet("/API/Order/GetOrder", (int? id) => controller.GetOrder(id));
}