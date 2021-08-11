using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveClinic.Consultation.Core.Tests.TestArtifacts;
using LiveClinic.Consultation.Infrastructure;
using MassTransit.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Consultation.Core.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;
        public static InMemoryTestHarness TestHarness;
        public static ConsumerTestHarness<TestOrderGeneratedHandler> TestConsumerOrderGenerated;

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
                .AddDbContext<ConsultationDbContext>(x => x.UseSqlite(connection));

            services.AddPersistence(config);
            services.AddEventBus(config, false);
            services.AddCore();
            ServiceProvider = services.BuildServiceProvider();
            ClearDb();
            SetupBus().Wait();
        }
        [OneTimeTearDown]
        public void End()
        {
            StopBus().Wait();
        }
        public static void ClearDb()
        {
            var context = ServiceProvider.GetService<ConsultationDbContext>();
            context.Database.EnsureCreated();
            context.EnsureSeeded();
        }
        public static void SeedData(params IEnumerable<object>[] entities)
        {
            var context = ServiceProvider.GetService<ConsultationDbContext>();

            foreach (IEnumerable<object> t in entities)
                context.AddRange(t);

            context.SaveChanges();
        }

        private  static async Task SetupBus()
        {
            TestHarness = ServiceProvider.GetService<InMemoryTestHarness>();
            TestConsumerOrderGenerated=TestHarness.Consumer<TestOrderGeneratedHandler>();
            await TestHarness.Start();
        }

        private  static async Task StopBus()
        {
            await TestHarness.Stop();
        }
    }
}
