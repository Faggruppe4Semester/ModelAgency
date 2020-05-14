﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModelsApi.Data;
using ModelsApi.Utilities;
using SplitListWebApi.Repositories.Interfaces;

namespace ModelsApi.Repositories.Implementation
{
    public class GenericRepository<TModel> : IRepository<TModel>
        where TModel : class
    {
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TModel> GetBy(
            Expression<Func<TModel, TModel>> selector,
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null,
            bool disableTracking = true)
        {
            return GeneralUtilities.GetFromDatabase(_dbContext, selector, predicate, orderBy, include, disableTracking);
        }

        public TModel Create(TModel model)
        {
            return model.WriteToDatabase(_dbContext.Add, _dbContext);
        }

        public TModel Update(TModel model)
        {
            return model.WriteToDatabase(_dbContext.Update, _dbContext);
        }

        public void Delete(TModel model)
        {
            model.WriteToDatabase(_dbContext.Remove, _dbContext);
        }
    }
}