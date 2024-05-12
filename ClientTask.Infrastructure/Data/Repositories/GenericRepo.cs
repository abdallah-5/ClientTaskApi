using ClientTask.Core.Interfaces.IRepositories;
using ClientTask.Core.Models;
using ClientTask.Core.Specifications;

using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ClientTask.Infrastructure.Data.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        public GenericRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }



        public async Task<IReadOnlyList<T>> ListAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> CreateEntityAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> CreateEntitiesAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }
        public T UpdateEntity(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void DeleteSoftEntity(T entity)
        {
            PropertyInfo isDeletedProperty = entity.GetType().GetProperty("IsDeleted");

            if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool?))
            {
                isDeletedProperty.SetValue(entity, true);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}