using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Other.Database.Cluster.Models;
using Mihaylov.Common.Abstract.Databases.Interfaces;
using Mihaylov.Api.Other.Database.Cluster.DbConfigurations;

namespace Mihaylov.Api.Other.Database.Cluster
{
    public class MihaylovOtherClusterDbContext : DbContext
    {
        public const string SCHEMA_NAME = "cluster";

        private readonly IAuditService _auditService;

        public MihaylovOtherClusterDbContext(DbContextOptions<MihaylovOtherClusterDbContext> options,
            IAuditService auditService) : base(options)
        {
            this._auditService = auditService;
        }

        public DbSet<Application> Applications { get; set; }

        public DbSet<ApplicationVersion> ApplicationVersions { get; set; }

        public DbSet<ApplicationPod> ApplicationPods { get; set; }

        public DbSet<DeploymentFile> DeploymentFiles { get; set; }

        public DbSet<Deployment> DeploymentTypes { get; set; }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInformation();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyAuditInformation();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(SCHEMA_NAME);

            builder.ApplyConfiguration(new ApplicationConfiguration());
            builder.ApplyConfiguration(new ApplicationVersionConfiguration());
            builder.ApplyConfiguration(new ApplicationPodConfiguration());
            builder.ApplyConfiguration(new DeploymentFileConfiguration());
            builder.ApplyConfiguration(new DeploymentConfiguration());

            base.OnModelCreating(builder);
        }

        private void ApplyAuditInformation()
        {
            var entities = ChangeTracker.Entries().ToList();

            _auditService.ApplyAuditInformation(entities);
        }
    }
}
