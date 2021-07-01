using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDBContext>(config => { config.UseInMemoryDatabase("Memory"); });

            // AddIdentity register the services
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                //config.Password.RequiredLength = 6;
                //config.SignIn.RequireConfirmedEmail = true;
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = default(bool);
                config.Password.RequireNonAlphanumeric = default(bool);
                config.Password.RequireUppercase = default(bool);
            })
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
            });

            services.AddIdentityServer().
                AddAspNetIdentity<IdentityUser>().
                AddInMemoryApiResources(Configuration.GetApis()).
                AddInMemoryIdentityResources(Configuration.GetIdentityResources()).
                AddInMemoryClients(Configuration.GetClients()).
                AddInMemoryApiScopes(Configuration.GetScopes()).
                AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
