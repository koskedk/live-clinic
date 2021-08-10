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
        Task<TC> GetAsync<TC,TId>(TId id) where TC : Entity<TId>;

        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<TC> GetAll<TC, TId>(Expression<Func<TC, bool>> predicate) where TC : Entity<TId>;

        Task<bool> ExistsAsync(T entity);
        Task<bool> ExistsAsync<TC, TId>(TC entity) where TC : Entity<TId>;

        Task CreateOrUpdateAsync(T entity);
        Task CreateOrUpdateAsync(IEnumerable<T> entities);
        Task CreateOrUpdateAsync<TC,TId>(IEnumerable<TC> entities) where TC : Entity<TId>;

        Task Delete(T entity);
        Task Delete(IEnumerable<T> entities);
        Task DeleteById(TId id);
        Task DeleteById(IEnumerable<TId> ids);
        Task DeleteById<TC, TCId>(IEnumerable<TCId> ids) where TC : Entity<TCId>;
    }
}
