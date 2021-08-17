using System;
using LiveClinic.Pharmacy.Core;
using LiveClinic.Pharmacy.Infrastructure;
using LiveClinic.SharedKernel.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace LiveClinic.Pharmacy
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
                        builder.WithOrigins("http://localhost:9000","http://localhost:9001")
                        //builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "LiveClinic.Pharmacy", Version = "v1"});
            });

            services.AddPersistence(Configuration);
            services.AddEventBus(Configuration, false);
                //.AddMassTransitHostedService();
            services.AddCore();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LiveClinic.Pharmacy v1"));
            }

            //  app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);
            // app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            EnsureMigrationOfContext<PharmacyDbContext>(app);
            Log.Information($"LiveCLINIC.Pharmacy [Version {GetType().Assembly.GetName().Version}] started successfully");

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
