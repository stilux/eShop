using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WarehouseService.Filters;
using WarehouseService.Providers;
using WarehouseService.Services;

namespace WarehouseService
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
            
            services.AddDbContext<WarehouseContext>(options
                => options.UseNpgsql(_configuration.GetConnectionString("WarehouseDBConnection")));
            
            services.AddScoped<IWarehouseService, Services.WarehouseService>();
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
            
            serviceProvider.GetRequiredService<WarehouseContext>().Database.Migrate();
        }
    }
}