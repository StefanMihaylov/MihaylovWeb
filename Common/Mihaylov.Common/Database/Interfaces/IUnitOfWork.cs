namespace Mihaylov.Common.Database.Interfaces
{
    public interface IUnitOfWork<C> where C : IDbContext
    {
        // Database Database { get; }

        // C Context { get; }

        int SaveChanges();
    }
}
