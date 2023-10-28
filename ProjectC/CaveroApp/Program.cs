using CaveroApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CaveroApp.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using YourNamespace.Services;
// using SendingEmails;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddTransient<SendingEmails.IEmailSender, EmailSender>();

// var emailService = new EmailService();
// emailService.SendEmail();

EmailService emailService = new EmailService();

string senderEmail = "your_email@gmail.com"; // Replace with your email
string senderPassword = "your_password"; // Replace with your email password
string recipientEmail = "recipient@example.com"; // Replace with recipient's email address

emailService.SendEmail(senderEmail, senderPassword, recipientEmail);

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