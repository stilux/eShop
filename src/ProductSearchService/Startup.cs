using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductSearchService.Filters;
using ProductSearchService.Models;
using ProductSearchService.Providers;
using ProductSearchService.Services;

namespace ProductSearchService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => { options.Filters.Add<ValidationFilter>(); })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            
            services.AddDbContext<ProductContext>(options
                => options.UseNpgsql(_configuration.GetConnectionString("ProductDBConnection")));
            
            services.AddScoped<IProductService, ProductService>();
            services.AddSingleton<IProductDbInitializerService, ProductDbInitializerService>();

            var cacheConfig = _configuration.GetSection("Cache");
            services.Configure<CacheConfig>(cacheConfig);

            services.AddMemoryCache(options =>
            {
                options.SizeLimit = cacheConfig.GetValue<long>("SizeLimit");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            
            serviceProvider.GetRequiredService<ProductContext>().Database.Migrate();

            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var productsCount = config.GetSection("DbInitialize").GetValue<int>("ItemsCount");
            serviceProvider.GetRequiredService<IProductDbInitializerService>().GenerateProducts(productsCount);
        }
    }
}