using System;
using LiveClinic.SharedKernel.Infrastructure.Repositories;
using LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts.Model;

namespace LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts
{
    public class TestCarRepository : BaseRepository<TestCar, Guid>, ITestCarRepository
    {
        public TestCarRepository(TestDbContext context) : base(context)
        {
        }
    }
}
