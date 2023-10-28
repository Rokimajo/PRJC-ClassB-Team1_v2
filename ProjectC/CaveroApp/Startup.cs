using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using YourNamespace.Services;

namespace YourNamespace
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Register your EmailService as a singleton
            services.AddSingleton<EmailService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Show detailed error pages in development
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Redirect to error page in production
                app.UseHsts(); // Use HSTS (HTTP Strict Transport Security) in production
            }

            app.UseRouting();
            // Add middleware configurations and routing

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Map controllers for MVC/Web API
            });
        }
    }
}
