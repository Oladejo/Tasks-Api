using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TaskWebApi.Services.Interfaces;

namespace TaskWebApi.Services.Implementations
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        /*
        * Code written by Oladejo B. Azeez.
        * oladejobabatundeazeez@gmail.com
        * */
        private DbContext Context { get; set; }

        DbSet<T> TSet { get; set; }

        public GenericRepository(DbContext ctx)
        {
            if (ctx == null)
                throw new Exception("Database context cannot be null");
            Context = ctx;
            // Context.Configuration.AutoDetectChangesEnabled = false;
            TSet = Context.Set<T>();
        }
        
        /// <summary>
        /// Adds the specified object to repository.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public void Add(T obj)
        {
            try
            {
                TSet.Add(obj);
                Context.SaveChanges();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Updates the specified object in repository.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public T Attach(T obj)
        {
            try
            {
                var id = typeof(T).GetProperty("Id").GetValue(obj);
                var previousObj = TSet.Find(id);
                Context.Entry<T>(previousObj).State = EntityState.Detached;
                TSet.Attach(obj);
                Context.Entry<T>(obj).State = EntityState.Modified;
                Context.SaveChanges();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Removes the specified object in repository.
        /// </summary>
        /// <param name="obj">object to be removed</param>
        /// <returns></returns>
        public bool Remove(T obj)
        {
            try
            {
                TSet.Remove(obj);
                Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="predicate">predicate to limit search.</param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return TSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets all object of type  T in repository.
        /// </summary>
        /// <param name="predicate">Optional predicate to limit search.</param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                if (predicate != null)
                {
                    return TSet.Where(predicate).AsEnumerable<T>();
                }
                else
                {
                    return TSet.AsEnumerable<T>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}