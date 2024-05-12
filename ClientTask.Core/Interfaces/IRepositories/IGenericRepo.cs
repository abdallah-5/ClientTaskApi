using ClientTask.Core.Models;
using ClientTask.Core.Specifications;

namespace ClientTask.Core.Interfaces.IRepositories
{
    public interface IGenericRepo<T> where T : BaseEntity
    {
        Task<T> GetById(int id);
        Task<IReadOnlyList<T>> ListAll();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);

        Task<T> CreateEntityAsync(T entity);
        Task<IEnumerable<T>> CreateEntitiesAsync(IEnumerable<T> entities);
        T UpdateEntity(T entity);
        Task SaveChangesAsync();

        void DeleteSoftEntity(T entity);
    }
}