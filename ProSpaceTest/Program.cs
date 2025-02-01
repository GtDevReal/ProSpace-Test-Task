using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using ProSpaceTest;
using ProSpaceTest.Data;
using ProSpaceTest.Data.Interfaces;
using ProSpaceTest.Data.Repositories;
using ProSpaceTest.Infrastructure;
using ProSpaceTest.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Manager", policy => policy.RequireRole("Manager"))
    .AddPolicy("Customer", policy => policy.RequireRole("Customer"));

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.ClaimsIdentity.RoleClaimType = System.Security.Claims.ClaimTypes.Role;

    opt.SignIn.RequireConfirmedPhoneNumber = false;
	opt.SignIn.RequireConfirmedEmail = false;
	opt.SignIn.RequireConfirmedAccount = false;

    opt.Password.RequireNonAlphanumeric = false;
	opt.Password.RequireDigit = false;
	opt.Password.RequireUppercase = false;
	opt.Password.RequireLowercase = false;

    opt.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Index";
    options.LogoutPath = "/Home/Logout";
});

builder.Services.AddAntiforgery(opt =>
{
    opt.HeaderName = "RequestVerificationToken";
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IConverterHelper, ConverterHelper>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await Initializer.InitializeRoles(services);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var staticFilesConfig = builder.Configuration.GetSection("StaticFiles:Paths").GetChildren();

foreach (var pathConfig in staticFilesConfig)
{
	var physicalPath = pathConfig["PhysicalPath"];
	var requestPath = pathConfig["RequestPath"];

	app.UseStaticFiles(new StaticFileOptions
	{
		FileProvider = new PhysicalFileProvider(
			Path.Combine(builder.Environment.ContentRootPath, physicalPath)),
		RequestPath = requestPath
	});
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "Manager",
    areaName: "Manager",
    pattern: "{controller=Manager}/{action=Dashboard}");

app.MapAreaControllerRoute(
	name: "Customer",
	areaName: "Customer",
	pattern: "{controller=Customer}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
