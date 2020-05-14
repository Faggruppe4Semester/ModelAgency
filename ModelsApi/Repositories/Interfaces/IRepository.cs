using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace SplitListWebApi.Repositories.Interfaces
{
    public interface IRepository<TModel>
    {
        public List<TModel> GetBy(Expression<Func<TModel, TModel>> selector,
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null,
            bool disableTracking = true);

        TModel Create(TModel entity);
        TModel Update(TModel model);
        void Delete(TModel entity);
    }
}