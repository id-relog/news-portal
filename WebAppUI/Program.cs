using BLL.Interfaces;
using DAL;
using BLL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebAppUI.Auth;

        
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<AgencyContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresConnection")));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase= false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
       



    })
    .AddEntityFrameworkStores<AgencyContext>()
    .AddDefaultTokenProviders();

    var connectionString = builder.Configuration.GetConnectionString("postgresConnection");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string 'postgresConnection' is not configured.");
    }
    builder.Services.ConfigureBLL(connectionString);



    builder.Services.AddControllersWithViews(); 
   

    var app = builder.Build();

   

    if (!app.Environment.IsDevelopment())
    {
    app.UseExceptionHandler("/Home/Error");
                
    app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();


    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
        
    

 //await RoleSeeder.EnsureRolesAsync(app.Services);
    //await AdminSeeder.EnsureDefaultAdminAsync(
    //    app.Services,
    //    builder.Configuration["DefaultAdmin:Email"] ?? string.Empty,
    //    builder.Configuration["DefaultAdmin:Password"] ?? string.Empty);