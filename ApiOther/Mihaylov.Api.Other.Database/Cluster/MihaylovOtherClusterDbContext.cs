using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Other.Database.Cluster.DbConfigurations;
using Mihaylov.Api.Other.Database.Cluster.Models;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Api.Other.Database.Cluster
{
    public class MihaylovOtherClusterDbContext : BaseDbContext<MihaylovOtherClusterDbContext>
    {
        public const string SCHEMA_NAME = "cluster";

        public MihaylovOtherClusterDbContext(DbContextOptions<MihaylovOtherClusterDbContext> options,
            IAuditService auditService) : base(options, auditService)
        {
        }

        public DbSet<Application> Applications { get; set; }

        public DbSet<ApplicationVersion> ApplicationVersions { get; set; }

        public DbSet<ApplicationPod> ApplicationPods { get; set; }

        public DbSet<DeploymentFile> DeploymentFiles { get; set; }

        public DbSet<Deployment> DeploymentTypes { get; set; }

        public DbSet<VersionUrl> VersionUrlTypes { get; set; }

        public DbSet<ParserSetting> ParserSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(SCHEMA_NAME);

            builder.ApplyConfiguration(new ApplicationConfiguration());
            builder.ApplyConfiguration(new ApplicationVersionConfiguration());
            builder.ApplyConfiguration(new ApplicationPodConfiguration());
            builder.ApplyConfiguration(new DeploymentFileConfiguration());
            builder.ApplyConfiguration(new DeploymentConfiguration());
            builder.ApplyConfiguration(new VersionUrlConfiguration());
            builder.ApplyConfiguration(new ParserSettingConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
