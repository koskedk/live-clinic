using System;
using LiveClinic.SharedKernel.Domain.Repositories;
using LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts.Model;

namespace LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts
{
    public interface ITestCarRepository : IRepository<TestCar, Guid>
    {

    }
}
