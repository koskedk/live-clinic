using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LiveClinic.SharedKernel.Domain.Repositories
{
    public interface IRepository<T, in TId> where T : AggregateRoot<TId>
    {
        Task<T> GetAsync(TId id);
        Task<TC> GetAsync<TC,TCId>(TCId id) where TC : Entity<TCId>;

        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllTracked(Expression<Func<T, bool>> predicate);
        IQueryable<TC> GetAll<TC, TCId>(Expression<Func<TC, bool>> predicate) where TC : Entity<TCId>;
        IQueryable<TC> GetAllTracked<TC, TCId>(Expression<Func<TC, bool>> predicate) where TC : Entity<TCId>;

        Task<bool> ExistsAsync(T entity);
        Task<bool> ExistsAsync<TC, TCId>(TC entity) where TC : Entity<TCId>;

        Task CreateOrUpdateAsync(T entity);
        Task CreateOrUpdateAsync(IEnumerable<T> entities);
        Task CreateOrUpdateAsync<TC,TCId>(IEnumerable<TC> entities) where TC : Entity<TCId>;

        Task UpdateAsync(T entity);

        Task Delete(T entity);
        Task Delete(IEnumerable<T> entities);
        Task DeleteById(TId id);
        Task DeleteById(IEnumerable<TId> ids);
        Task DeleteById<TC, TCId>(IEnumerable<TCId> ids) where TC : Entity<TCId>;
    }
}
