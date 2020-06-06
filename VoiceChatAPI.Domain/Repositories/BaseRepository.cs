using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoiceChatAPI.Domain.Interfaces;

namespace VoiceChatAPI.Domain.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly VCDbContext Db;

        protected BaseRepository(VCDbContext db)
        {
            Db = db;
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await Db.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAll()
        {
            return await Db.Set<T>().ToListAsync();
        }

        public async Task<T> GetSingleBySpec(ISpecification<T> spec)
        {
            var result = await List(spec);
            return result.FirstOrDefault();
        }

        public IQueryable<T> QueryWithIncludes(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return secondaryResult;
        }

        public async Task<List<T>> List(ISpecification<T> spec)
        {
            // return the result of the query using the specification's criteria expression
            return await QueryWithIncludes(spec)
                            .Where(spec.Criteria)
                            .ToListAsync();
        }

        public async Task<List<T>> List(ISpecification<T> spec, int offset, int limit)
        {
            // return the result of the query using the specification's criteria expression
            return await QueryWithIncludes(spec)
                            .Where(spec.Criteria)
                            .Skip((offset - 1) * limit)
                            .Take(limit)
                            .ToListAsync();
        }

        public async Task<T> Add(T entity)
        {
            Db.Set<T>().Add(entity);
            await Db.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
            Db.Entry(entity).State = EntityState.Modified;
            await Db.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            Db.Set<T>().Remove(entity);
            await Db.SaveChangesAsync();
        }

        public async Task ReloadAsync(T entity)
        {
            await Db.Entry(entity).ReloadAsync();
        }

        public async Task<List<T>> AddList(List<T> listEntity)
        {
            if (listEntity.Any())
            {
                foreach (var item in listEntity)
                {
                    Db.Set<T>().Add(item);
                }
                await Db.SaveChangesAsync();
            }
            return listEntity;
        }
    }
}
