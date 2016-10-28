using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TaskWebApi.Services.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        /// <summary>
        /// Adds the specified object to repository.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        void Add(T obj);

        /// <summary>
        /// Updates the specified object in repository.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        T Attach(T obj);
        /// <summary>
        /// Removes the specified object in repository.
        /// </summary>
        /// <param name="Id">The identifier of object to be removed.</param>
        /// <returns></returns>
        bool Remove(T obj);

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="predicate"> predicate to limit search.</param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets all object of type  T in repository.
        /// </summary>
        /// <param name="predicate">Optional predicate to limit search.</param>
        /// <returns></returns>
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);
    }
}
