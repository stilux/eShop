using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WarehouseService.Filters;
using WarehouseService.Infrastructure;
using WarehouseService.Services;
using MassTransit;
using WarehouseService.Configs;

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
            ConfigureMassTransit(services);
            
            services.AddControllers(options => { options.Filters.Add<ValidationFilter>(); })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            
            services.AddDbContext<WarehouseContext>(options
                => options.UseNpgsql(_configuration.GetConnectionString("WarehouseDBConnection"), sqlOption =>
                {
                    sqlOption.EnableRetryOnFailure(10);
                }));
            
            services.AddScoped<IWarehouseService, Services.WarehouseService>();
            services.AddSingleton<IWarehouseDbInitializerService, WarehouseDbInitializerService>();
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var massTransitSettingSection = _configuration.GetSection("MassTransitConfig");
            var massTransitConfig = massTransitSettingSection.Get<MassTransitConfig>();
            
            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetExecutingAssembly());
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.Host(massTransitConfig.Host, massTransitConfig.VirtualHost,
                        h =>
                        {
                            h.Username(massTransitConfig.Username);
                            h.Password(massTransitConfig.Password);
                        }
                    );
                });
            });
            
            services.AddMassTransitHostedService();
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
            
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var count = config.GetSection("DbInitialize").GetValue<int>("ItemsCount");
            serviceProvider.GetRequiredService<IWarehouseDbInitializerService>().GenerateWarehouseItems(count);
        }
    }
}