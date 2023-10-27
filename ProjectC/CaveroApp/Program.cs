using CaveroApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CaveroApp.Data;
using CaveroApp.Seeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// using postgres for identity database
builder.Services.AddDbContext<CaveroAppContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<CaveroAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<CaveroAppContext>();
builder.Services.AddRazorPages();
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

app.Run();