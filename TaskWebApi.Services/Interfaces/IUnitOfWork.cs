using System;

namespace TaskWebApi.Services.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        /// <summary>
        /// Saves the changes in the repositories.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Gets all repositories attach to this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> Repository<T>() where T : class;
    }
}
