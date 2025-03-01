using InventoryManagementSystem.Services;
using InventoryMannagementSystem.Business_Layer.BusinessInterface;
using InventoryMannagementSystem.Business_Layer.BusinessService;
using InventoryMannagementSystem.Interface;
using InventoryMannagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Temp";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // set Time Out
    options.Cookie.HttpOnly = true; // Prevents Client-side Access
    options.Cookie.IsEssential = true; // Ensure session Works Even Without Consent
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IVendor, VendorService>();
builder.Services.AddScoped<ICategory, CategoryServices>();
builder.Services.AddScoped<IProduct, ProductServices>();
builder.Services.AddScoped<ICustomer, CustomerServices>();


builder.Services.AddScoped<IBusinessVendor, BusinessVendorClass>();
builder.Services.AddScoped<IBusinessCustomer, BusinessCustomerClass>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
