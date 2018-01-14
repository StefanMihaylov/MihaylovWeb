namespace Mihaylov.Common.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Mihaylov.Common.Database.Interfaces;

    public abstract class BaseUnitOfWork<C> : IUnitOfWork<C> where C : IDbContext
    {
        private readonly IDictionary<Type, Type> repositoryTypes;
        private readonly IDictionary<Type, object> repositories;

        public BaseUnitOfWork(C context)
        {
            this.Context = context;
            this.repositoryTypes = new Dictionary<Type, Type>();
            this.repositories = new Dictionary<Type, object>();

            this.AddRepositoryTypes(this.repositoryTypes);
        }

        public C Context { get; private set; }

        public Database Database
        {
            get { return this.Context.Database; }
        }

        /// <summary>
        /// Key - DTO type, Value - Repository type
        /// </summary>
        /// <param name="repositoryTypes"></param>
        protected abstract void AddRepositoryTypes(IDictionary<Type, Type> repositoryTypes);

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Context != null)
                {
                    this.Context.Dispose();
                }
            }
        }

        protected virtual IRepository<T> GetRepository<T>() where T : class
        {
            Type type = typeof(T);
            if (!this.repositories.ContainsKey(type))
            {
                foreach (Type modelType in repositoryTypes.Keys)
                {
                    if (type.IsAssignableFrom(modelType))
                    {
                        Type repositoryType = repositoryTypes[modelType];
                        this.AddRepository(type, repositoryType);
                        break;
                    }
                }
            }

            return (IRepository<T>)this.repositories[type];
        }

        protected virtual void AddRepository(Type type, Type repositoryType)
        {
            object instance = Activator.CreateInstance(repositoryType, this.Context);
            this.repositories.Add(type, instance);
        }
    }
}
