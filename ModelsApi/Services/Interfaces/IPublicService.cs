using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModelsApi.Services.Interfaces
{
    public interface IPublicService<TSource, T>
        where TSource : class
    {
        Task<List<TSource>> GetAll();
        List<TSource> GetBy(Expression<Func<T, bool>> predicate);
        Task<TSource> Create(TSource dto);
        Task Update(long id, TSource dto);
        Task Delete(long id);
    }
}