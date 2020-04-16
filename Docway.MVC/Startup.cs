using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Docway.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options => {
                options.DefaultScheme = "cookie_mvc_docway";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("cookie_mvc_docway", options =>
            {
                // Set cookie to expire in X minutes.
                options.ExpireTimeSpan = System.TimeSpan.FromMinutes(1);
            })
            .AddOpenIdConnect("oidc", options => {
                options.SignInScheme = "cookie_mvc_docway";
                
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;
                
                options.ClientId = "mvc1";
                options.ClientSecret = "segredo";
                options.ResponseType = "code id_token token";
                
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                
                options.Scope.Add("mvc1");
                options.Scope.Add("offline_access");
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
