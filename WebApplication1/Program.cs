/*
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MyConnectionString")
    ?? throw new InvalidOperationException("Connection string 'MyConnectionString' not found.");

builder.Services.AddDbContext<RecipeWebDbContext>(options =>
    options.UseSqlServer(connectionString));

// Ensure the WebApplication1Context is configured
var identityConnectionString = builder.Configuration.GetConnectionString("WebApplication1ContextConnection")
    ?? throw new InvalidOperationException("Connection string 'WebApplication1ContextConnection' not found.");

builder.Services.AddDbContext<WebApplication1Context>(options =>
    options.UseSqlServer(identityConnectionString));

builder.Services.AddDefaultIdentity<WebApplication1User>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<WebApplication1Context>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication middleware is used
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RecipeWeb}/{action=HomePage}/{id?}");

app.Run();
 */


using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MyConnectionString") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// Ensure the WebApplication1Context is configured
var identityConnectionString = builder.Configuration.GetConnectionString("WebApplication1ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication1ContextConnection' not found.");

builder.Services.AddDbContext<RecipeWebDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<WebApplication1Context>(options => options.UseSqlServer(identityConnectionString));

builder.Services.AddDefaultIdentity<WebApplication1User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebApplication1Context>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("connectionOnly", policy => policy.RequireAuthenticatedUser());
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
name: "default",
pattern: "{controller=RecipeWeb}/{action=HomePage}/{id?}");

app.MapRazorPages();
app.Run();

