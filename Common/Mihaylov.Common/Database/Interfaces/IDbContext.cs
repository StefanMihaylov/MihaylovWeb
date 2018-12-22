namespace Mihaylov.Common.Database.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;   
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public interface IDbContext
    {
        DatabaseFacade Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry Entry(object entity);

        int SaveChanges();

        void Dispose();
    }
}
