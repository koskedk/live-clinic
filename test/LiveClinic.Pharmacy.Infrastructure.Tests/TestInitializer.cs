using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Infrastructure.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;

        [OneTimeSetUp]
        public void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json", false, true)
                .Build();

            var connectionString = config.GetConnectionString("DatabaseConnection");
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            var services = new ServiceCollection()
                .AddDbContext<PharmacyDbContext>(x => x.UseSqlite(connection));

            services.AddPersistence(config);
            services.AddEventBus(config, false);

            ServiceProvider = services.BuildServiceProvider();

            ClearDb();
        }

        public static void ClearDb()
        {
            var context = ServiceProvider.GetService<PharmacyDbContext>();
            context.Database.EnsureCreated();
            context.EnsureSeeded();
        }
        public static void SeedData(params IEnumerable<object>[] entities)
        {
            var context = ServiceProvider.GetService<PharmacyDbContext>();

            foreach (IEnumerable<object> t in entities)
                context.AddRange(t);

            context.SaveChanges();
        }
    }
}
