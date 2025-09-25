
using Microsoft.EntityFrameworkCore;
using NextUses.Data;
using Microsoft.AspNetCore.Identity;
using NextUses.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();  // IHttpContextAccessor inject ???? ????
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // session active ???
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();


// Use environment variable or fallback to appsettings.json connection string
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                       ?? builder.Configuration.GetConnectionString("NextUsesDBConnectionString");

builder.Services.AddDbContext<NextUsesDB>(options =>
    options.UseSqlServer(connectionString));

// Identity setup
builder.Services.AddIdentity<Users, IdentityRole>()
    .AddEntityFrameworkStores<NextUsesDB>()
    .AddDefaultTokenProviders();

// Bind MailSettings from configuration (supports env vars override automatically)

// Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LogoutPath = "/Account/Logout";
});

var app = builder.Build();

// Seed roles on startup
async Task SeedRoles(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "Admin", "Rider", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedRoles(roleManager);
}

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Prevent caching
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});



// MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Frontend}/{action=Index}/{id?}");



app.MapControllerRoute(
    name: "backend",
    pattern: "Backend/{action=index}/{id?}",
    defaults: new { controller = "Backend" });

app.Run();
