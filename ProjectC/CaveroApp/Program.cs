using CaveroApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CaveroApp.Data;
using CaveroApp.Seeder;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// using postgres for identity database
builder.Services.AddDbContext<CaveroAppContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<CaveroAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CaveroAppContext>();
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust the timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<CaveroApp.Services.EventServices>();

var app = builder.Build();

//seeding optional here
IServiceProvider serviceProvider = app.Services;
using (var scope = serviceProvider.CreateScope())
{
    // var seeder = new Seeder(scope.ServiceProvider);
    // seeder.Seed();
}


//Automatically go to login screen on app run
app.MapGet("/", () =>
{
    return Results.Redirect("/Identity/Account/Login");
});

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
app.UseSession();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CaveroAppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Check if roles exist and create them if they don't
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    // Check if users exist and assign roles
    var adminUser = await userManager.FindByNameAsync("admin@cavero.nl");
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    var caveriaanUser = await userManager.FindByNameAsync("user@cavero.nl");
    if (caveriaanUser != null && !await userManager.IsInRoleAsync(caveriaanUser, "User"))
    {
        await userManager.AddToRoleAsync(caveriaanUser, "User");
    }
}

app.Run();