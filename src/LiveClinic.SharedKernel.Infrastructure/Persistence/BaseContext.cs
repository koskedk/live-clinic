using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LiveClinic.SharedKernel.Infrastructure.Persistence
{
    public abstract class BaseContext : DbContext
    {
        private IDbContextTransaction _transaction;
        protected BaseContext(DbContextOptions options) : base(options)
        {
        }
        public virtual void EnsureSeeded()
        {
        }
        public virtual void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }
        public virtual void Commit()
        {
            try
            {
                SaveChanges();
                _transaction.Commit();
            }
            finally
            {
               _transaction.Dispose();
            }
        }
        public virtual void RollBack()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

    }
}
