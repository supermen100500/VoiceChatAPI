using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VoiceChatAPI.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetById(Guid id);
        Task<List<T>> ListAll();
        Task<T> GetSingleBySpec(ISpecification<T> spec);
        Task<List<T>> List(ISpecification<T> spec);
        Task<List<T>> List(ISpecification<T> spec, int offset, int limit);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task ReloadAsync(T entity);
        Task Delete(T entity);
        Task<List<T>> AddList(List<T> listEntity);
    }
}
