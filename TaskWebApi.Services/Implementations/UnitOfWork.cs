using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskWebApi.Services.Interfaces;
using TaskWebApi.Services.Models;

namespace TaskWebApi.Services.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        /*
         * Code written by Oladejo B. Azeez.
         * oladejobabatundeazeez@gmail.com
         * */

        private readonly DbContext _context;
        public UnitOfWork()
        {
            _context = new TaskApiDBContext();
        }

        /// <summary>
        /// a Dictionary<typeparamref name="Type"/> of repositories as key and class object 
        /// </summary>
        public Dictionary<Type, object> Repositories = new Dictionary<Type, object>();
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        
        /// <summary>
        /// Saves the changes in the repositories.
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets all repositories attach to this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> Repository<T>() where T : class
        {

            if (Repositories.Keys.Contains(typeof(T)) == true)
            {
                return Repositories[typeof(T)] as IRepository<T>;
            }
            IRepository<T> trepo = new GenericRepository<T>(_context);
            Repositories.Add(typeof(T), trepo);
            return trepo;
        }
    }
}