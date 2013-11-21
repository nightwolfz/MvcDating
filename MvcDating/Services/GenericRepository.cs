using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Domain.Models;
using EntityState = System.Data.Entity.EntityState;

namespace MvcDating.Services
{
    public class GenericRepository<T> where T : class
    {
        internal UsersContext Context;
        internal DbSet<T> DbSet;

        public GenericRepository(UsersContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null) query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null) return orderBy(query).ToList();
                        
            return query.ToList();
        }

        public virtual T Single(Expression<Func<T, bool>> filter)
        {
            return DbSet.SingleOrDefault(filter);
        }

        /// <summary>
        /// Find by key id
        /// </summary>
        public virtual T Find(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached) DbSet.Attach(entityToDelete);
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}