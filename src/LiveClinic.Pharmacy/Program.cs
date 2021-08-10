using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace LiveClinic.Pharmacy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("obs.json", optional: false)
                .Build();
            var seqUrl = config.GetSection("Seq").Get<string>();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("SERVICE","LiveCLINIC.Pharmacy")
                .WriteTo.Console(LogEventLevel.Debug)
                //.WriteTo.File("logs/log.txt", LogEventLevel.Error,rollingInterval: RollingInterval.Day)
                .WriteTo.Seq(seqUrl,LogEventLevel.Information)
                .CreateLogger();

            try
            {
                Log.Information($"Starting LiveBILL.Import...");;
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(GetConfig(args));
                    webBuilder.UseStartup<Startup>();
                });

        private static IConfigurationRoot GetConfig(string[] args)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("hosting.json", optional: true)
                .AddCommandLine(args).Build();
        }
    }
}
