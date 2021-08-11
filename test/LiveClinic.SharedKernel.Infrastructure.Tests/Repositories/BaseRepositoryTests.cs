using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts;
using LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts.Model;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.SharedKernel.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class BaseRepositoryTests
    {
        private ITestCarRepository _testEntityRepository;
        private List<TestCar> _testEntities;

        [OneTimeSetUp]
        public void Init()
        {
            _testEntities = TestCar.CreateTestCars(20);
            TestInitializer.ClearDb();
            TestInitializer.SeedData(_testEntities);
        }

        [SetUp]
        public void SetUp()
        {
            _testEntityRepository = TestInitializer.ServiceProvider.GetService<ITestCarRepository>();
        }

        [Test]
        public void should_GetAsync()
        {
            var testCar = _testEntityRepository.GetAsync(_testEntities.First().Id).Result;
            Assert.NotNull(testCar);
            Log.Debug($"{testCar}");
        }

        [Test]
        public void should_GetAsync_Generic()
        {
            var testCar = _testEntityRepository.GetAsync<TestCar,Guid>(_testEntities.First().Id).Result;
            Assert.NotNull(testCar);
            Log.Debug($"{testCar}");
        }


        [Test]
        public void should_Get_All()
        {
            var testEntities = _testEntityRepository.GetAll().ToList();
            Assert.True(testEntities.Count > 0);
            foreach (var testEntity in testEntities)
            {
                Log.Debug(testEntity.ToString());
            }
        }

        [Test]
        public void should_Get_All_By_Predicate()
        {
            var testName = _testEntities.First().Name;

            var testEntities = _testEntityRepository.GetAll(x => x.Name.ToLower().Contains(testName.ToLower())).ToList();
            Assert.True(testEntities.Count > 0);
            foreach (var testCar in testEntities)
            {
                Log.Debug(testCar.ToString());
            }
        }

        [Test]
        public void should_Get_All_By_Predicate_Tracked()
        {
            var testName = _testEntities.First().Name;

            var testEntities = _testEntityRepository.GetAllTracked(x => x.Name.ToLower().Contains(testName.ToLower())).ToList();
            Assert.True(testEntities.Count > 0);
            foreach (var testCar in testEntities)
            {
                Log.Debug(testCar.ToString());
            }
        }

        [Test]
        public void should_Get_All_Generic_By_Predicate()
        {
            var testName = _testEntities.Last().TestCarModels.First().Name;

            var testEntities = _testEntityRepository.GetAll<TestCarModel,Guid>(x => x.Name.ToLower().Contains(testName.ToLower())).ToList();
            Assert.True(testEntities.Count > 0);
            foreach (var testCarModel in testEntities)
            {
                Log.Debug(testCarModel.ToString());
            }
        }

        [Test]
        public void should_Get_All_Generic_By_Predicate_Tracked()
        {
            var testName = _testEntities.Last().TestCarModels.First().Name;

            var testEntities = _testEntityRepository.GetAllTracked<TestCarModel,Guid>(x => x.Name.ToLower().Contains(testName.ToLower())).ToList();
            Assert.True(testEntities.Count > 0);
            foreach (var testCarModel in testEntities)
            {
                Log.Debug(testCarModel.ToString());
            }
        }

        [Test]
        public void should_Check_If_Exists()
        {
            var testEntity = _testEntities.Last();
            var testFound = _testEntityRepository.ExistsAsync(testEntity).Result;
            Assert.True(testFound);
        }

        [Test]
        public void should_Check_If_Generic_Exists()
        {
            var testEntity = _testEntities.Last().TestCarModels.Last();
            var testFound = _testEntityRepository.ExistsAsync<TestCarModel,Guid>(testEntity).Result;
            Assert.True(testFound);
        }

        [Test]
        public void should_CreateOrUpdate_New()
        {
            var testEntity = new TestCar("Velar");
            _testEntityRepository.CreateOrUpdateAsync(testEntity).Wait();

            var newTestEntity = _testEntityRepository.GetAsync(testEntity.Id).Result;
            Assert.NotNull(newTestEntity);
            Log.Debug(newTestEntity.ToString());
        }

        [Test]
        public void should_CreateOrUpdate_Generic_New()
        {
            var tid = _testEntities.First().Id;
            var testCarModel = new TestCarModel("m", 2019, new TestTrim("T", "P"), tid);
            _testEntityRepository.CreateOrUpdateAsync<TestCarModel, Guid>(new[]{testCarModel}).Wait();


            var carModel = _testEntityRepository.GetAsync<TestCarModel, Guid>(testCarModel.Id).Result;
            Assert.NotNull(carModel);
            Log.Debug(carModel.ToString());
        }

        [Test]
        public void should_CreateOrUpdate_Multi_New()
        {
            var t1 = new TestCar("CarA");
            var t2 = new TestCar("CarB");
            var testCars = new List<TestCar> {t1, t2};
            _testEntityRepository.CreateOrUpdateAsync(testCars).Wait();

            _testEntityRepository = TestInitializer.ServiceProvider.GetService<ITestCarRepository>();
            var newTestEntity = _testEntityRepository.GetAsync(t1.Id).Result;
            Assert.NotNull(newTestEntity);
            var newTestEntity2 = _testEntityRepository.GetAsync(t2.Id).Result;
            Assert.NotNull(newTestEntity2);
            Log.Debug(newTestEntity.ToString());
            Log.Debug(newTestEntity2.ToString());
        }

        [Test]
        public void should_CreateOrUpdate_Multi_Generic_New()
        {

            var tid = _testEntities.First().Id;
            var testCarModels = Builder<TestCarModel>.CreateListOfSize(2).All().With(x => x.TestCarId = tid)
                .With(x => x.Trim = new TestTrim("MT", "P")).Build()
                .ToList();

            _testEntityRepository.CreateOrUpdateAsync<TestCarModel, Guid>(testCarModels).Wait();

            _testEntityRepository = TestInitializer.ServiceProvider.GetService<ITestCarRepository>();
            var testCarModel = _testEntityRepository.GetAsync<TestCarModel, Guid>(testCarModels.First().Id).Result;
            Assert.NotNull(testCarModel);
            var carModel = _testEntityRepository.GetAsync<TestCarModel, Guid>(testCarModels.Last().Id).Result;
            Assert.NotNull(carModel);
            Log.Debug(testCarModel.ToString());
            Log.Debug(carModel.ToString());
        }

        [Test]
        public void should_CreateOrUpdate_Existing()
        {
            var testEntityForUpdate = _testEntities.First();
            testEntityForUpdate.Name = "GLE Benz";

            _testEntityRepository.CreateOrUpdateAsync(testEntityForUpdate).Wait();

            var updatedTestEntity = _testEntityRepository.GetAsync(testEntityForUpdate.Id).Result;
            Assert.AreEqual("GLE Benz", updatedTestEntity.Name);
            Log.Debug(updatedTestEntity.ToString());
        }

        [Test]
        public void should_CreateOrUpdate_Multi_Existing()
        {
            var testEntityForUpdate = _testEntities.First();
            testEntityForUpdate.Name = "GLE Benz";
            var testEntityForUpdate2 = _testEntities.Last();
            testEntityForUpdate2.Name = "x5 BMW";
            var testCars = new List<TestCar> {testEntityForUpdate, testEntityForUpdate2};

            _testEntityRepository.CreateOrUpdateAsync(testCars).Wait();

            var updatedTestEntity = _testEntityRepository.GetAsync(testEntityForUpdate.Id).Result;
            Assert.AreEqual("GLE Benz", updatedTestEntity.Name);
            var updatedTestEntity2 = _testEntityRepository.GetAsync(testEntityForUpdate2.Id).Result;
            Assert.AreEqual("x5 BMW", updatedTestEntity2.Name);
            Log.Debug(updatedTestEntity.ToString());
            Log.Debug(updatedTestEntity2.ToString());
        }

        [Test]
        public void should_CreateOrUpdate_Multi_Generic_Existing()
        {
            var testEntityForUpdate = _testEntities.First().TestCarModels.First();
            testEntityForUpdate.Name = "GLE Benz";
            var testEntityForUpdate2 = _testEntities.Last().TestCarModels.Last();
            testEntityForUpdate2.Name = "x5 BMW";
            var testCarModels = new List<TestCarModel> {testEntityForUpdate, testEntityForUpdate2};

            _testEntityRepository.CreateOrUpdateAsync<TestCarModel, Guid>(testCarModels).Wait();


            var updatedTestEntity =
                _testEntityRepository.GetAsync<TestCarModel, Guid>(testEntityForUpdate.Id).Result;
            Assert.AreEqual("GLE Benz", updatedTestEntity.Name);
            var updatedTestEntity2 =
                _testEntityRepository.GetAsync<TestCarModel, Guid>(testEntityForUpdate2.Id).Result;
            Assert.AreEqual("x5 BMW", updatedTestEntity2.Name);
            Log.Debug(updatedTestEntity.ToString());
            Log.Debug(updatedTestEntity2.ToString());
        }

        [Test]
        public void should_Delete_Existing()
        {
            var entities = TestCar.CreateTestCars();
            TestInitializer.SeedData(entities);

            var testEntityForDelete = entities.Last();
            _testEntityRepository.Delete(testEntityForDelete);

            var deletedTestEntity = _testEntityRepository.GetAsync(testEntityForDelete.Id).Result;
            Assert.IsNull(deletedTestEntity);
        }


        [Test]
        public void should_Delete_Multiple_Existing()
        {
            var entities = TestCar.CreateTestCars();
            TestInitializer.SeedData(entities);

            _testEntityRepository.Delete(entities);

            var deletedTestEntity = _testEntityRepository.GetAsync(entities.First().Id).Result;
            var deletedTestEntity2 = _testEntityRepository.GetAsync(entities.Last().Id).Result;
            Assert.IsNull(deletedTestEntity);
            Assert.IsNull(deletedTestEntity2);
        }

        [Test]
        public void should_Delete_Existing_By_Id()
        {
            var entities = TestCar.CreateTestCars();
            TestInitializer.SeedData(entities);

            var testEntityForDelete = entities.Last();
            _testEntityRepository.DeleteById(testEntityForDelete.Id);

            var deletedTestEntity = _testEntityRepository.GetAsync(testEntityForDelete.Id).Result;
            Assert.IsNull(deletedTestEntity);
        }

        [Test]
        public void should_Delete_Multiple_Existing_By_Id()
        {
            var entities = TestCar.CreateTestCars();
            TestInitializer.SeedData(entities);

            _testEntityRepository.DeleteById(entities.Select(x => x.Id).ToList());

            var deletedTestEntity = _testEntityRepository.GetAsync(entities.First().Id).Result;
            var deletedTestEntity2 = _testEntityRepository.GetAsync(entities.Last().Id).Result;
            Assert.IsNull(deletedTestEntity);
            Assert.IsNull(deletedTestEntity2);
        }

        [Test]
        public void should_Delete_Generic_Multiple_Existing_By_Id()
        {
            var entities = TestCar.CreateTestCars();
            TestInitializer.SeedData(entities);

            _testEntityRepository.DeleteById<TestCar,Guid>(entities.First().TestCarModels.Select(x => x.Id).ToList());

            var deletedTestEntity = _testEntityRepository.GetAsync(entities.First().TestCarModels.First().Id)
                .Result;
            var deletedTestEntity2 = _testEntityRepository.GetAsync(entities.First().TestCarModels.Last().Id)
                .Result;
            Assert.IsNull(deletedTestEntity);
            Assert.IsNull(deletedTestEntity2);
        }
    }
}
