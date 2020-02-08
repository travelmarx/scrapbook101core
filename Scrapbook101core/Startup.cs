using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

namespace Scrapbook101core
{
    /// <summary>
    /// Configures services and the application's request pipeline. 
    /// All ASP.NET Core apps use a Startup class, which is named Startup by convention.
    /// For more information, see 
    /// <see href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.0">App startup in ASP.NET Core</see>.
    /// </summary>
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
            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add functionality to enable build-time compilation of razor pages
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            // Add our Config object so it can be injected
            services.Configure<Scrapbook101Configuration>(Configuration.GetSection("Scrapbook101Config"));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //https://weblog.west-wind.com/posts/2017/Dec/12/Easy-Configuration-Binding-in-ASPNET-Core-revisited
            var config = new Scrapbook101Configuration();
            Configuration.Bind("Scrapbook101Config", config);

            // Create singleton to access config parameters easily.
            services.AddSingleton(config);
            // Initialize repository.
            DocumentDBRepository<Scrapbook101core.Models.Item>.Initialize();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Scrapbook101 API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scrapbook101 API V1");
            });

            if (env.EnvironmentName.Equals("Development"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // For the wwwroot
            app.UseStaticFiles(); 
            // For the Assets
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Assets")),
                RequestPath = "/Assets"
            });
            app.UseRouting();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Item}/{action=Index}/{id?}");
            });
        }
    }
}
