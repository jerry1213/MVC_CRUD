using DataAccess;
using Microsoft.EntityFrameworkCore;
using Service;
using Platform;
using YungChing_MVC;
using Microsoft.OpenApi.Models;
using System.Reflection;
using DataAccess.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

#region DI setting (Add class into container)
// Service 
builder.Services.RegisterService();

// DataAccess
builder.Services.RegisterDataAccess(builder);

// Platform
builder.Services.RegisterPlatform();

// Web
builder.Services.RegisterWeb();
#endregion

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers(); // This line ensures that the API controllers are mapped

app.Run();
