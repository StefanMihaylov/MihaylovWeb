namespace Mihaylov.Common.Interfaces
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IDbContext
    {
        Database Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry Entry(object entity);

        int SaveChanges();

        void Dispose();
    }
}
