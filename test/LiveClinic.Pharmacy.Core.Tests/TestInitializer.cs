using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using LiveClinic.Pharmacy.Core.Tests.TestArtifacts;
using LiveClinic.Pharmacy.Infrastructure;
using MassTransit.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;
        public static InMemoryTestHarness TestHarness;
        public static ConsumerTestHarness<TestOrderFulfilledHandler> TestConsumerOrderFulfilled;
        public static ConsumerTestHarness<TestOrderAcceptedHandler> TestConsumerOrderAccepted;
        public static ConsumerTestHarness<TestOrderRejectedHandler> TestConsumerOrderRejected;

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
            services.AddEventBus(config, false,typeof(TestOrderAcceptedHandler));
            services.AddCore(new List<Assembly>(){ typeof(TestOrderValidatedEventHandler).Assembly});

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

        private  static async Task SetupBus()
        {
            TestHarness = ServiceProvider.GetService<InMemoryTestHarness>();
            TestConsumerOrderFulfilled = TestHarness.Consumer<TestOrderFulfilledHandler>();
            TestConsumerOrderAccepted = TestHarness.Consumer<TestOrderAcceptedHandler>();
            TestConsumerOrderRejected = TestHarness.Consumer<TestOrderRejectedHandler>();

            await TestHarness.Start();
        }

        private  static async Task StopBus()
        {
            await TestHarness.Stop();
        }
    }
}
