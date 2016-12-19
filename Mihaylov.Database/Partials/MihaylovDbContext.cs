using Mihaylov.Database.Models.Interfaces;

namespace Mihaylov.Database
{
    public partial class MihaylovDbContext : IMihaylovDbContext
    {
        public MihaylovDbContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}