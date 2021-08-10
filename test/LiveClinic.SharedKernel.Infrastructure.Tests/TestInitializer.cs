using System;
using System.Collections.Generic;
using LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.SharedKernel.Infrastructure.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;

        [OneTimeSetUp]
        public void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("SQLiteInMemory");

            var connection = new SqliteConnection(connectionString);
            connection.Open();

            var services = new ServiceCollection().AddDbContext<TestDbContext>(x => x.UseSqlite(connection));

            services.AddTransient<TestDbContext>();
            services.AddTransient<ITestCarRepository, TestCarRepository>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public static void ClearDb()
        {
            var context = ServiceProvider.GetService<TestDbContext>();
            context.Database.EnsureCreated();
            context.EnsureSeeded();
        }
        public static void SeedData(params IEnumerable<object>[] entities)
        {
            var context = ServiceProvider.GetService<TestDbContext>();

            foreach (IEnumerable<object> t in entities)
            {
                context.AddRange(t);
            }

            context.SaveChanges();
        }
    }
}
