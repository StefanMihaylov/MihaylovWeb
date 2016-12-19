namespace Mihaylov.Common.Base
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Mihaylov.Common.Interfaces;

    public abstract class BaseUnitOfWork<C> : IUnitOfWork<C> where C : IDbContext
    {
        private readonly C context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public BaseUnitOfWork(C context)
        {
            this.context = context;
        }

        public C Context
        {
            get
            {
                return this.context;
            }
        }

        public Database Database
        {
            get
            {
                return this.Context.Database;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        protected virtual IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!this.ContainsRepository(type))
            {
                this.AddRepository(type, typeof(GenericRepository<T, C>));
            }

            return (IRepository<T>)this.repositories[type];
        }

        protected virtual void AddRepository(Type type, Type repositoryType)
        {
            var instance = Activator.CreateInstance(repositoryType, this.context);
            this.repositories.Add(type, instance);
        }

        protected virtual bool ContainsRepository(Type type)
        {
            return this.repositories.ContainsKey(type);
        }
    }
}
