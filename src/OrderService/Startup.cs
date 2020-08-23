using System;
using System.Reflection;
using MassTransit;
using OrderService.Filters;
using OrderService.Providers;
using OrderService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.Configs;
using OrderService.Sagas.OrderProcessingSaga;
using Shared.Contracts.Messages;

namespace OrderService
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
            
            services.AddDbContext<OrderContext>(options
                => options.UseNpgsql(_configuration.GetConnectionString("OrderDBConnection")));
            
            services.AddScoped<IOrderService, OrderService.Services.OrderService>();
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var massTransitSettingSection = _configuration.GetSection("MassTransitConfig");
            var massTransitConfig = massTransitSettingSection.Get<MassTransitConfig>();
            
            services.AddMassTransit(i =>
            {              
                i.AddConsumers(Assembly.GetExecutingAssembly());
                i.AddActivities(Assembly.GetExecutingAssembly());
                
                var timeout = TimeSpan.FromSeconds(10);
                i.AddRequestClient<IReserveProducts>(timeout);
                i.AddRequestClient<ICancelReservation>(timeout);
                i.AddRequestClient<IOrderPayment>(timeout);
                i.AddRequestClient<ICancelPayment>(timeout);
                i.AddRequestClient<IDeliveryRequest>(timeout);
                
                i.SetKebabCaseEndpointNameFormatter();
                i.AddSagaStateMachine<OrderStateMachine, OrderState>()                
                    .InMemoryRepository();
                i.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseInMemoryOutbox();
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
            
            serviceProvider.GetRequiredService<OrderContext>().Database.Migrate();
        }
    }
}