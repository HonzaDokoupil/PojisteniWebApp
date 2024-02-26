using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PojisteniWebApp.Classes;
using PojisteniWebApp.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
// zaregistujeme PDFGenerator do Depedency Injection
builder.Services.AddSingleton<PDFGenerator>();
// zaregistrujeme Info Tøídu
builder.Services.AddScoped<Info>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (IServiceScope scope = app.Services.CreateScope())
{
    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    if(!await roleManager.RoleExistsAsync(UserRoles.Admin))
    {
        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
    }
    if (!await roleManager.RoleExistsAsync(UserRoles.Insured))
    {
        await roleManager.CreateAsync(new IdentityRole(UserRoles.Insured));
    }
    IdentityUser? defaultAdminUser = await userManager.FindByEmailAsync("admin@doky.cz");
    if (defaultAdminUser == null)
    {
        IdentityUser user = new IdentityUser { UserName = "admin@doky.cz", Email = "admin@doky.cz" };    
        IdentityResult result = await userManager.CreateAsync(user, "Heslojeveslo1");
        defaultAdminUser = await userManager.FindByEmailAsync("admin@doky.cz");
    }
    if (defaultAdminUser != null && !await userManager.IsInRoleAsync(defaultAdminUser, UserRoles.Admin))
    {
        await userManager.AddToRoleAsync(defaultAdminUser, UserRoles.Admin);
    }
}

app.Run();


