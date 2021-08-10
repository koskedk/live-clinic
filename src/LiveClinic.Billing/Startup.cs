using System;
using LiveClinic.Billing.Core;
using LiveClinic.Billing.Core.Application.Invoicing.EventHandlers;
using LiveClinic.Billing.Infrastructure;
using LiveClinic.Billing.Infrastructure.Persistence;
using LiveClinic.SharedKernel.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using LiveClinic.SharedKernel.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace LiveClinic.Billing
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        //builder.WithOrigins("http://localhost:6001","https://localhost:6443")
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "LiveClinic.Billing", Version = "v1"});
            });

            services.AddPersistence(Configuration);
            services.AddCore();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var positionOptions = new RabbitMqOptions();
            Configuration.GetSection(RabbitMqOptions.RabbitMq).Bind(positionOptions);

            services.AddMassTransit(mt =>
            {
                mt.SetKebabCaseEndpointNameFormatter();
                mt.AddConsumersFromNamespaceContaining<DrugOrderValidatedHandler>();
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(positionOptions.Host, positionOptions.VirtualHost, h =>
                        {
                            h.Username(positionOptions.Username);
                            h.Password(positionOptions.Password);
                        }
                    );
                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LiveClinic.Billing v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            EnsureMigrationOfContext<BillingDbContext>(app);
            Log.Information($"LiveCLINIC.Billing [Version {GetType().Assembly.GetName().Version}] started successfully");

        }

        private static void EnsureMigrationOfContext<T>(IApplicationBuilder app) where T : BaseContext
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contextName = typeof(T).Name;
                Log.Debug($"initializing Database context: {contextName}");
                var context =serviceScope.ServiceProvider.GetService<T>();
                try
                {
                    context.Database.Migrate();
                    context.EnsureSeeded();
                    Log.Debug($"initializing Database context: {contextName} [OK]");
                }
                catch (Exception e)
                {
                    var msg = $"initializing Database context: {contextName} Error";
                    Log.Error(e, msg);
                }
            }
        }
    }
}
