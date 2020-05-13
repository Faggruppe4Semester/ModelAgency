using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using ModelsApi.Data;

namespace ModelsApi.Utilities
{
    public static class GeneralUtilities
    {
        //Encapsulate Db-funcitions into transactions.
        public static T WriteToDatabase<T>(this T source, Func<T, EntityEntry<T>> dbFunc, ApplicationDbContext db) where T : class
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var entry = dbFunc(source);
                    db.SaveChanges();
                    transaction.Commit();
                    return entry.Entity;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public static List<TResult> GetFromDatabase<TResult, TEntity>(
            ApplicationDbContext db,
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        where TEntity : class
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).ToList();
            }

            return query.Select(selector).ToList();
        }
    }
}